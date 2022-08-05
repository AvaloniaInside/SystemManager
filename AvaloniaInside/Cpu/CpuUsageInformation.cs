namespace AvaloniaInside;

public class CpuUsageInformation
{
    /// <summary>
    ///     Currently usage in percent.
    /// </summary>
    public double Usage { get; internal set; }

    /// <summary>
    ///     State.
    /// </summary>
    public CpuUsageState State { get; internal set; } = CpuUsageState.Unavailable;
}