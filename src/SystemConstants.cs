namespace AvaloniaInside.SystemManager;

public static class SystemConstants
{
    internal const string Sh = "/bin/sh";
    internal const string NetworkInterfaces = "/etc/network/interfaces";
    internal const string HostName = "/proc/sys/kernel/hostname";
    internal const string NetworkService = "S40network";
    internal const string DropBearService= "S50dropbear";
    internal const string SshdService = "S50sshd";
    internal const string InitD = "/etc/init.d/";
    internal const string ProcStat = "/proc/stat";
    internal const string MemInfo = "/proc/meminfo";
    internal const string Thermal = "/sys/class/thermal/";
}