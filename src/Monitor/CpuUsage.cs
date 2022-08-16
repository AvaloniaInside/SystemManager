namespace AvaloniaInside.SystemManager.Monitor;

public sealed class CpuUsage : IntervalCollection<CpuUsageInformation>
{
    private List<(float[] values, int core)> _prevCoreInfo = new();

    public override CpuUsageInformation Current { get; protected set; } = new();

    protected override async Task<CpuUsageInformation> NewValueAsync(CancellationToken cancellationToken)
    {
        var statLines = await File.ReadAllLinesAsync("/proc/stat", cancellationToken);
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

        if (float.IsNaN(Current.Usage))
        {
            Current.Usage = 0;
            for (var i = 0; i < Current.Cores.Length; i++) Current.Cores[i] = 0;
        }

        return Current;
    }

    protected override void Disposing()
    {
        base.Disposing();
        _prevCoreInfo.Clear();
        _prevCoreInfo = null;
    }
}