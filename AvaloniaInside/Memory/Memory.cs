namespace AvaloniaInside;

public static class Memory
{
    public delegate void MemoryUsageStateChangedHandler(MemoryUsageStateChangedEventArgs e);

    public delegate void MemoryUsageUpdatedHandler(EventArgs e);

    /// <summary>
    ///     Free system memory
    /// </summary>
    public static long Free { get; set; }

    /// <summary>
    ///     Available system memory
    /// </summary>
    public static long Total { get; set; }

    /// <summary>
    ///     Used system memory
    /// </summary>
    public static double Usage => (float)Free / Total;

    /// <summary>
    ///     State.
    /// </summary>
    public static MemoryUsageState State { get; internal set; } = MemoryUsageState.Ok;

    /// <summary>
    ///     Event when the <see cref="State" /> is changed
    /// </summary>
    public static event MemoryUsageStateChangedHandler? MemoryUsageStateChanged;

    /// <summary>
    ///     Event when the memory information's updated
    /// </summary>
    public static event MemoryUsageUpdatedHandler? MemoryUsageUpdated;

    /// <summary>
    ///     Gets the memory usage from /proc/meminfo
    /// </summary>
    public static void UpdateMemoryUsageInformation()
    {
        foreach (var line in File.ReadLines("/proc/meminfo"))
        {
            var (property, value) = GetMemInfoLineData(line);
            switch (property)
            {
                case "MemTotal":
                    Total = value;
                    break;
                case "MemFree":
                    Free = value;
                    break;
            }
        }

        var newState = EvaluateMemoryUsageState();
        if (newState != State)
            MemoryUsageStateChanged?.Invoke(new MemoryUsageStateChangedEventArgs(State, newState));
        State = newState;
        MemoryUsageUpdated?.Invoke(EventArgs.Empty);
    }

    /// <summary>
    ///     Extract Header and Value from /proc/meminfo line
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static (string, int) GetMemInfoLineData(string line)
    {
        var propertyRead = false;
        var property = "";
        var value = "";
        foreach (var c in line)
        {
            if (c == ':')
                propertyRead = true;
            else if(!propertyRead)
                property += c;

            if (!propertyRead) continue;
            if (char.IsNumber(c))
                value += c;
        }

        return (property, Convert.ToInt32(value));
    }

    /// <summary>
    ///     Calculate the <see cref="State" /> based on <see cref="Settings.MemoryUsageWarningLevel" /> and
    ///     <see cref="Settings.MemoryUsageOverloadLevel" />
    /// </summary>
    /// <returns></returns>
    private static MemoryUsageState EvaluateMemoryUsageState()
    {
        if (Usage >= Settings.MemoryUsageOverloadLevel)
            return MemoryUsageState.Overload;
        if (Usage >= Settings.MemoryUsageWarningLevel)
            return MemoryUsageState.Warning;
        return MemoryUsageState.Ok;
    }
}