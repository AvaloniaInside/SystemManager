namespace AvaloniaInside.SystemManager.Monitor;

public sealed class CpuUsage : IAsyncEnumerable<CpuUsageInformation>, IAsyncEnumerator<CpuUsageInformation>
{
    private bool _isStarted;
    private bool _isDisposed;
    private List<(float[] values, int core)> _prevCoreInfo = new();
    private CancellationToken _cancellationToken;

    public CpuUsageInformation Current { get; } = new();
    /// <summary>
    /// Interval of cpu usage check in milliseconds, recommended to use default value, 1000 
    /// </summary>
    public int Interval { get; set; } = 1000;

    public IAsyncEnumerator<CpuUsageInformation> GetAsyncEnumerator(
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
        var statLines = await File.ReadAllLinesAsync("/proc/stat", _cancellationToken);
        var query =
            from line in statLines
            where line.StartsWith("cpu")
            let columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            let coreString = columns.First().Substring(3)
            let core = coreString.Length > 0 ? int.Parse(coreString) : -1
            select (columns.Skip(1).Select(float.Parse).ToArray(), core);
        var output = query.ToList();
        if (_prevCoreInfo.Count != output.Count)
        {
            _prevCoreInfo.Clear();
            _prevCoreInfo.AddRange(output);
        }

        var join =
            from coreInfo in output
            join prev in _prevCoreInfo on coreInfo.core equals prev.core
            select new
            {
                Percent = 100.0f * (1.0f - (coreInfo.Item1[3] - prev.values[3]) /
                    (coreInfo.Item1.Sum() - prev.values.Sum())),
                Core = coreInfo.core
            };

        var percentage = join.OrderBy(o => o.Core).ToList();
        Current.Usage = percentage.First().Percent;
        Current.Cores = percentage.Skip(1).Select(s => s.Percent).ToArray();
        
        _prevCoreInfo.Clear();
        _prevCoreInfo.AddRange(output);

        return true;
    }
}