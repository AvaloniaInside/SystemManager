namespace AvaloniaInside;

public static class Settings
{
    public static bool NetworkOperationStateDetectionEnabled { get; set; } = true;
    public static string DefaultNetworkInterface { get; set; } = "eth0";
    public static SshType SshType { get; set; } = SshType.Dropbear;
    public static bool CpuUsageWatcherEnabled { get; set; } = true;
    public static double MemoryUsageWarningLevel { get; set; } = 65.0;
    public static double MemoryUsageOverloadLevel { get; set; } = 85.0;
}