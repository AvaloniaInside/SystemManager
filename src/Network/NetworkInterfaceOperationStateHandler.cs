namespace AvaloniaInside.SystemManager;

internal static class NetworkInterfaceOperationStateHandler
{
    internal static NetworkInterfaceOperationState GetNetworkInterfaceOperationState(string networkInterface)
    {
        try
        {
            Bash.Execute($"cat /sys/class/net/{networkInterface}/operstate",
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
}