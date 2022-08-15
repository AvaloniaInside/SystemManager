namespace AvaloniaInside.Monitor;

public class CpuUsageInformation
{
    public float Usage { get; internal set; }
    public float[] Cores { get; internal set; } = Array.Empty<float>();
}