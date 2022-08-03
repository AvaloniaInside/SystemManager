namespace AvaloniaInside.SSH;

public static class Ssh
{
    /// <summary>
    ///     Starts the network daemon.
    /// </summary>
    public static void StartDaemon() => SystemService.Start("S50sshd");

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopDaemon() => SystemService.Stop("S50sshd");
}