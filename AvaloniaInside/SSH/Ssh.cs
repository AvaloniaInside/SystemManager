using System.Diagnostics;

namespace AvaloniaInside;

public static class Ssh
{
    private static readonly string DropBearServiceFileName = "S50dropbear";
    private static readonly string SshdServiceFileName = "S50sshd";

    /// <summary>
    ///     Determines if any ssh service is running.
    /// </summary>
    public static bool IsRunning => CheckIfAnySshDaemonIsRunning();

    /// <summary>
    ///     Starts the network daemon.
    /// </summary>
    public static void StartDaemon()
    {
        if (IsRunning)
            return;
        SystemService.Start(GetSshTypeServiceName());
    }

    /// <summary>
    ///     Gets the ServiceFileName based on <see cref="Settings.SshType" />
    /// </summary>
    /// <returns></returns>
    private static string GetSshTypeServiceName()
    {
        return Settings.SshType == SshType.Dropbear ? DropBearServiceFileName : SshdServiceFileName;
    }

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopDaemon()
    {
        var detectedRunningType = GetCurrentRunningSshType();
        if (detectedRunningType == null || detectedRunningType == Settings.SshType)
            return; // no running ssh service found
        SystemService.Stop(detectedRunningType is SshType.Dropbear ? DropBearServiceFileName : SshdServiceFileName);
    }

    /// <summary>
    ///     Determines if any ssh service is running.
    /// </summary>
    public static bool CheckIfAnySshDaemonIsRunning()
    {
        return Process.GetProcesses().Any(x => x.ProcessName is "drobear" or "/usr/sbin/sshd");
    }

    /// <summary>
    ///     Detects the current running SshType. Returns null of no ssh service is running.
    /// </summary>
    /// <returns></returns>
    public static SshType? GetCurrentRunningSshType()
    {
        if (Process.GetProcessesByName("dropbear").Length > 0)
            return SshType.Dropbear;
        if (Process.GetProcessesByName("/usr/sbin/sshd").Length > 0)
            return SshType.Sshd;
        return null;
    }
}