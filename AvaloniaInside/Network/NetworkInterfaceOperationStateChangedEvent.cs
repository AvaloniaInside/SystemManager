namespace AvaloniaInside;

public class NetworkInterfaceOperationStateChangedEvent : EventArgs
{
    internal NetworkInterfaceOperationStateChangedEvent(NetworkInterfaceOperationState interfaceState)
    {
        NewOperationState = interfaceState;
    }

    public NetworkInterfaceOperationState NewOperationState { get; set; }
}