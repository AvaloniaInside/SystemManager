namespace AvaloniaInside;

public static class Cpu
{
    public delegate void CpuUsageStateChangedHandler(CpuUsageStateChangedEventArgs eventargs);

    public static CpuUsageInformation OverallUsage { get; } = new();
    public static CpuUsageInformation Core0Usage { get; } = new();
    public static CpuUsageInformation Core1Usage { get; } = new();
    public static CpuUsageInformation Core2Usage { get; } = new();
    public static CpuUsageInformation Core3Usage { get; } = new();

    /// <summary>
    ///     Event when the <see cref="CpuUsageState" /> of a core or the cpu change.
    /// </summary>
    public static event CpuUsageStateChangedHandler? CpuUsageStateChanged;

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
            var columns = line.Split(' ');
            switch (columns[0])
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
    }

    /// <summary>
    ///     Calculate cpu usage per core
    /// </summary>
    /// <param name="columns"></param>
    /// <returns></returns>
    private static double CalculateCpuUsage(string[] columns)
    {
        var col2 = Convert.ToDouble(columns[2]);
        var col4 = Convert.ToDouble(columns[4]);
        var col5 = Convert.ToDouble(columns[5]);
        return (col2 + col4) * 100 / (col2 + col4 + col5);
    }

    /// <summary>
    /// Updates the <see cref="CpuUsageInformation"/> for a core or the cpu
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