using System.Collections.Concurrent;

namespace SystemManager;

public class Tail : IAsyncEnumerable<string>, IAsyncEnumerator<string>
{
	private readonly string _path;
	private readonly bool _continuously;
	private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
	private string? _current;
	private readonly CancellationTokenSource _tokenSource = new();
	private readonly ConcurrentQueue<string?> _queue = new();
	private bool _alreadySetup;

	private CancellationToken Token => _tokenSource.Token;

	public Tail(string path, bool continuously)
	{
		_path = path;
		_continuously = continuously;
	}

	public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = new())
	{
		cancellationToken.Register(_tokenSource.Cancel);
		if (_alreadySetup) throw new InvalidOperationException("Already executed! Launch new tail");

		_alreadySetup = true;
		_ = Task.Run(SetupStreamAsync, Token);

		return this;
	}

	private async Task SetupStreamAsync()
	{
		await using var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using var sr = new StreamReader(fs);

		while (!Token.IsCancellationRequested)
		{
			var line = await sr.ReadLineAsync();
			if (line != null)
			{
				_queue.Enqueue(line);
				_semaphoreSlim.Release();
			}
			else if (_continuously)
			{
				await Task.Delay(500, Token);
			}
			else
			{
				_tokenSource.Cancel();
			}
		}
	}

	public ValueTask DisposeAsync()
	{
		_tokenSource.Dispose();
		return ValueTask.CompletedTask;
	}

	public async ValueTask<bool> MoveNextAsync()
	{
		if (_queue.IsEmpty)
		{
			await _semaphoreSlim.WaitAsync(Token);
		}

		return _queue.TryDequeue(out _current);
	}

	public string Current => _current ?? throw new ArgumentNullException(nameof(_current));
}

public static class TailExtensions
{
	public static IAsyncEnumerable<string> Tail(this string path) => new Tail(path, true);
	public static IAsyncEnumerable<string> Cat(this string path) => new Tail(path, false);
}
