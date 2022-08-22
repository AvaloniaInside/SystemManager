namespace AvaloniaInside.SystemManager;

public static class Network
{
    public delegate void NetworkInterfaceOperationStateChangedHandler(
        NetworkInterfaceOperationStateChangedEventArgs eventargs);

    /// <summary>
    ///     Information about the network configuration
    /// </summary>
    public static NetworkInformation? Information { get; private set; }

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
                new NetworkInterfaceOperationStateChangedEventArgs(newState));
            DefaultInterfaceOperationState = newState;
        }
    }

    public static NetworkInformation? GetNetworkInformation(string networkInterface)
    {
        Information = NetworkHandler.GetNetworkInformation(networkInterface);
        return Information;
    }

    /// <summary>
    ///     Gets the
    ///     <see cref="NetworkInterfaceOperationState" />
    ///     for an ethernet interface
    /// </summary>
    /// <param name="networkInterface"></param>
    /// <returns></returns>
    public static NetworkInterfaceOperationState GetNetworkInterfaceOperationState(string networkInterface)
    {
        return NetworkInterfaceOperationStateHandler.GetNetworkInterfaceOperationState(networkInterface);
    }

    /// <summary>
    ///     Starts the network daemon.
    /// </summary>
    public static void StartDaemon()
    {
        SystemService.Start(SystemConstants.NetworkService);
    }

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopDaemon()
    {
        SystemService.Stop(SystemConstants.NetworkService);
    }

    /// <summary>
    ///     Sets the hostname
    /// </summary>
    /// <param name="hostName"></param>
    public static void SetHostName(string hostName)
    {
        NetworkHandler.SetHostName(hostName);
    }
}