namespace AvaloniaInside;

public static class Settings
{
    public static bool NetworkOperationStateDetectionEnabled { get; set; } = true;
    public static string DefaultNetworkInterface { get; set; } = "eth0";
    public static SshType SshType { get; set; } = SshType.Dropbear;
    public static bool CpuUsageWatcherEnabled { get; set; } = true;
}