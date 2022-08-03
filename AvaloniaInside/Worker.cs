namespace AvaloniaInside;

internal static class Worker
{
    static Worker()
    {
        Task.Factory.StartNew(() =>
        {
            if (Settings.NetworkOperationStateDetectionEnabled) 
                Network.CheckDefaultNetworkInterfaceOperationState();

            Thread.Sleep(1000);
        });
    }
}