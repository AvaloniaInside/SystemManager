using AvaloniaInside.SystemManager.Helpers;

namespace AvaloniaInside.SystemManager.Monitor;

public class MemoryUsage : IntervalCollection<MemoryUsageInformation>
{
    private readonly string[] _columnToRead = new[] { "MemTotal", "MemFree", "SwapTotal", "SwapFree" };

    public override MemoryUsageInformation Current { get; protected set; } = new();

    protected override async Task<MemoryUsageInformation> NewValueAsync(CancellationToken cancellationToken)
    {
        var meminfoLines = await File.ReadAllLinesAsync("/proc/meminfo", cancellationToken);
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

        Current.State = EvaluateMemoryUsageState(Current.MemoryFree / Current.MemorySize);
        return Current;
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