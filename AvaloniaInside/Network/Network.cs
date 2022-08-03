namespace AvaloniaInside;

public static class Network
{
    public delegate void NetworkInterfaceOperationStateChangedHandler(
        NetworkInterfaceOperationStateChangedEvent eventargs);

    private static CancellationTokenSource? _networkCheckerCancellationTokenSource;
    public static NetworkInterfaceOperationState DefaultInterfaceOperationState { get; private set; }

    /// <summary>
    ///     Event when the OperationState of the <see cref="Settings.DefaultNetworkInterface" /> change.
    /// </summary>
    public static event NetworkInterfaceOperationStateChangedHandler? NetworkInterfaceOperationStateChanged;

    /// <summary>
    ///     Starts the NetworkOperationStateChecker.
    /// </summary>
    public static void StartNetworkOperationStateChecker()
    {
        if (_networkCheckerCancellationTokenSource != null)
            return;
        _networkCheckerCancellationTokenSource = new CancellationTokenSource();
        new Task(() =>
            {
                while (!_networkCheckerCancellationTokenSource.IsCancellationRequested)
                {
                    var newState = GetNetworkInterfaceOperationState(Settings.DefaultNetworkInterface);
                    if (newState != DefaultInterfaceOperationState)
                    {
                        NetworkInterfaceOperationStateChanged?.Invoke(
                            new NetworkInterfaceOperationStateChangedEvent(newState));
                        DefaultInterfaceOperationState = newState;
                    }

                    Thread.Sleep(1000);
                }
            }, _networkCheckerCancellationTokenSource.Token)
            .Start();
    }

    /// <summary>
    ///     Stops the NetworkOperationStateChecker.
    /// </summary>
    public static void StopNetworkOperationStateChecker()
    {
        if (_networkCheckerCancellationTokenSource == null) return;
        _networkCheckerCancellationTokenSource.Cancel();
        _networkCheckerCancellationTokenSource.Dispose();
        _networkCheckerCancellationTokenSource = null;
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
    public static void StartNetworkDaemon()
    {
        try
        {
            Bash.Execute("/etc/init.d/S40network", "start",
                out var error, out _);
            if (!string.IsNullOrEmpty(error)) Console.WriteLine($"StartNetworkDaemon error: {error}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StartNetworkDaemon error: {ex.Message}");
        }
    }

    /// <summary>
    ///     Stops the network daemon.
    /// </summary>
    public static void StopNetworkDaemon()
    {
        try
        {
            Bash.Execute("/etc/init.d/S40network", "stop",
                out var error, out _);
            if (!string.IsNullOrEmpty(error)) Console.WriteLine($"StopNetworkDaemon error: {error}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StopNetworkDaemon error: {ex.Message}");
        }
    }
}