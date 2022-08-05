namespace AvaloniaInside;

public static class Network
{
    public delegate void NetworkInterfaceOperationStateChangedHandler(
        NetworkInterfaceOperationStateChangedEvent eventargs);

    private static string _hostName;

    public static string HostName
    {
        get
        {
            if (string.IsNullOrEmpty(_hostName))
                _hostName = GetHostName();
            return _hostName;
        }
        set
        {
            SetHostName(value);
            _hostName = value;
        }
    }

    public static NetworkInterfaceOperationState DefaultInterfaceOperationState { get; private set; }

    /// <summary>
    ///     Event when the OperationState of the <see cref="Settings.DefaultNetworkInterface" /> change.
    /// </summary>
    public static event NetworkInterfaceOperationStateChangedHandler? NetworkInterfaceOperationStateChanged;

    /// <summary>
    ///     Check if the OperationState of the <see cref="Settings.DefaultNetworkInterface" /> has changed.
    ///     If changed <see cref="NetworkInterfaceOperationStateChanged" /> is raised.
    /// </summary>
    internal static void CheckDefaultNetworkInterfaceOperationState()
    {
        var newState = GetNetworkInterfaceOperationState(Settings.DefaultNetworkInterface);
        if (newState != DefaultInterfaceOperationState)
        {
            NetworkInterfaceOperationStateChanged?.Invoke(
                new NetworkInterfaceOperationStateChangedEvent(newState));
            DefaultInterfaceOperationState = newState;
        }
    }

    /// <summary>
    ///     Gets the <see cref="NetworkInterfaceOperationState" /> for an ethernet interface
    /// </summary>
    /// <param name="networkInterface"></param>
    /// <returns></returns>
    public static NetworkInterfaceOperationState GetNetworkInterfaceOperationState(string networkInterface)
    {
        try
        {
            Bash.Execute("/bin/cat/", $"/sys/class/net/{networkInterface}/operstate",
                out var error, out var output);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"CheckIfEthernetIsUp error: {error}");
                return NetworkInterfaceOperationState.Unavailable;
            }

            output = output.Trim().Replace(Environment.NewLine, "");
            return output == "up" ? NetworkInterfaceOperationState.Up : NetworkInterfaceOperationState.Down;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CheckIfEthernetIsUp error: {ex.Message}");
            return NetworkInterfaceOperationState.Unavailable;
        }
    }

    /// <summary>
    ///     Starts the network daemon.
    /// </summary>
    public static void StartDaemon()
    {
        SystemService.Start("S40network");
    }

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopDaemon()
    {
        SystemService.Stop("S40network");
    }

    /// <summary>
    ///     Gets the hostname
    /// </summary>
    /// <param name="hostName"></param>
    private static void SetHostName(string hostName)
    {
        File.WriteAllText("/proc/sys/kernel/hostname", hostName);
    }

    /// <summary>
    ///     Sets the hostname
    /// </summary>
    private static string GetHostName()
    {
        return File.ReadAllText("/proc/sys/kernel/hostname");
    }
}