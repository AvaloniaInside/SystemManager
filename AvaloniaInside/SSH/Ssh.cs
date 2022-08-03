using System.Diagnostics;

namespace AvaloniaInside.SSH;

public static class Ssh
{
    /// <summary>
    ///     Starts the network daemon.
    /// </summary>
    public static void StartDaemon()
    {
        SystemService.Start("S50dropbear");
    }

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopDaemon()
    {
        SystemService.Stop("S50dropbear");
    }

    private static bool CheckIfSshDaemonIsRunning()
    {
        return Process.GetProcessesByName("dropbear").Any();
    }
}