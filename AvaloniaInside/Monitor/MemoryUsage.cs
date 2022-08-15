using AvaloniaInside.Helpers;

namespace AvaloniaInside.Monitor;

public class MemoryUsage : IAsyncEnumerable<MemoryUsageInformation>, IAsyncEnumerator<MemoryUsageInformation>
{
    private readonly string[] _columnToRead = new[] { "MemTotal", "MemFree", "SwapTotal", "SwapFree" };
    
    private bool _isStarted;
    private bool _isDisposed;
    private List<(float[] values, int core)> _prevCoreInfo = new();
    private CancellationToken _cancellationToken;

    public MemoryUsageInformation Current { get; } = new();
    /// <summary>
    /// Interval of memory usage check in milliseconds, recommended to use default value, 1000 
    /// </summary>
    public int Interval { get; set; } = 1000;
    
    public IAsyncEnumerator<MemoryUsageInformation> GetAsyncEnumerator(
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (_isDisposed) throw new ObjectDisposedException("Object already disposed");
        
        _cancellationToken = cancellationToken;
        if (_isStarted) throw new InvalidOperationException("Already is running. Create another instance");
        _isStarted = true;

        return this;
    }

    public ValueTask DisposeAsync()
    {
        if (_isDisposed) return ValueTask.CompletedTask;
        _isDisposed = true;
        _prevCoreInfo.Clear();
        _prevCoreInfo = null;
        return ValueTask.CompletedTask;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        if (_isDisposed) throw new ObjectDisposedException("Object already disposed");
        if (!_isStarted) throw new InvalidOperationException("Bad usage, Please use async foreach to start operations");

        await Task.Delay(Interval, _cancellationToken).ConfigureAwait(false);
        
        var meminfoLines = await File.ReadAllLinesAsync("/proc/meminfo", _cancellationToken);
        var items = meminfoLines
            .Select(s => s.Split(':', 2))
            .Where(w => w.Length == 2);

        foreach (var item in items)
        {
            switch (Array.IndexOf(_columnToRead, item[0]))
            {
                case 0: Current.MemorySize = ByteHelper.Parse(item[1]); break;
                case 1: Current.MemoryFree = ByteHelper.Parse(item[1]); break;
                case 2: Current.SwapSize = ByteHelper.Parse(item[1]); break;
                case 3: Current.SwapFree = ByteHelper.Parse(item[1]); break;
            }
        }

        Current.State = EvaluateMemoryUsageState(Current.MemorySize - Current.MemoryFree);

        return true;
    }

    
    /// <summary>
    ///     Calculate the <see cref="State" /> based on <see cref="Settings.MemoryUsageWarningLevel" /> and
    ///     <see cref="Settings.MemoryUsageOverloadLevel" />
    /// </summary>
    /// <returns></returns>
    private static MemoryUsageState EvaluateMemoryUsageState(ulong usage)
    {
        if (usage >= Settings.MemoryUsageOverloadLevel)
            return MemoryUsageState.Overload;
        if (usage >= Settings.MemoryUsageWarningLevel)
            return MemoryUsageState.Warning;
        return MemoryUsageState.Ok;
    }
}