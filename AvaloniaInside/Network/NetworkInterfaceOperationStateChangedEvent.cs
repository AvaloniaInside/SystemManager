namespace AvaloniaInside;

public class NetworkInterfaceOperationStateChangedEventArgs : EventArgs
{
    internal NetworkInterfaceOperationStateChangedEventArgs(NetworkInterfaceOperationState interfaceState)
    {
        NewOperationState = interfaceState;
    }

    public NetworkInterfaceOperationState NewOperationState { get; set; }
}