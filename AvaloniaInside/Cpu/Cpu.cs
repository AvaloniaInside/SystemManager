namespace AvaloniaInside;

public static class Cpu
{
    public delegate void CpuUsageStateChangedHandler(CpuUsageStateChangedEventArgs e);

    public delegate void CpuUsageUpdatedHandler(EventArgs e);

    static Cpu()
    {
        UpdateCpuUsageInformation();
    }

    public static CpuUsageInformation OverallUsage { get; } = new();
    public static CpuUsageInformation Core0Usage { get; } = new();
    public static CpuUsageInformation Core1Usage { get; } = new();
    public static CpuUsageInformation Core2Usage { get; } = new();
    public static CpuUsageInformation Core3Usage { get; } = new();

    /// <summary>
    ///     Event when the <see cref="CpuUsageState" /> of a core or the cpu change.
    /// </summary>
    public static event CpuUsageStateChangedHandler? CpuUsageStateChanged;

    public static event CpuUsageUpdatedHandler? CpuUsageUpdated;

    /// <summary>
    ///     Gets the cpu usage from /proc/stat
    /// </summary>
    public static void UpdateCpuUsageInformation()
    {
        var overallChanged = false;
        var core0Changed = false;
        var core1Changed = false;
        var core2Changed = false;
        var core3Changed = false;
        foreach (var line in File.ReadLines("/proc/stat"))
        {
            var columns = line.Split(new[] { "  ", " " }, StringSplitOptions.None);
            switch (columns[0].Trim())
            {
                case "cpu":
                    UpdateCpuUsageInformation(columns, OverallUsage, CalculateCpuUsage(columns), out overallChanged);
                    break;
                case "cpu0":
                    UpdateCpuUsageInformation(columns, Core0Usage, CalculateCpuUsage(columns), out core0Changed);
                    break;
                case "cpu1":
                    UpdateCpuUsageInformation(columns, Core1Usage, CalculateCpuUsage(columns), out core1Changed);
                    break;
                case "cpu2":
                    UpdateCpuUsageInformation(columns, Core2Usage, CalculateCpuUsage(columns), out core2Changed);
                    break;
                case "cpu3":
                    UpdateCpuUsageInformation(columns, Core3Usage, CalculateCpuUsage(columns), out core3Changed);
                    break;
            }
        }

        if (overallChanged || core0Changed || core1Changed || core2Changed || core3Changed)
            CpuUsageStateChanged?.Invoke(new CpuUsageStateChangedEventArgs());
        CpuUsageUpdated?.Invoke(EventArgs.Empty);
    }

    /// <summary>
    ///     Calculate cpu usage per core
    /// </summary>
    /// <param name="columns"></param>
    /// <returns></returns>
    private static double CalculateCpuUsage(string[] columns)
    {
        var col1 = Convert.ToDouble(columns[1]);
        var col3 = Convert.ToDouble(columns[3]);
        var col4 = Convert.ToDouble(columns[4]);
        return Math.Round((col1 + col3) * 100 / (col1 + col3 + col4), 2);
    }

    /// <summary>
    ///     Updates the <see cref="CpuUsageInformation" /> for a core or the cpu
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="information"></param>
    /// <param name="newUsage"></param>
    /// <param name="stateChanged"></param>
    private static void UpdateCpuUsageInformation(string[] columns, CpuUsageInformation information, double newUsage,
        out bool stateChanged)
    {
        information.Usage = CalculateCpuUsage(columns);
        var newState = CpuUsageState.Ok;
        stateChanged = newState != information.State;
        information.State = newState;
    }
}