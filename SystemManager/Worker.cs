namespace SystemManager;

internal class Worker
{
    internal void StartEndlessLoop()
    {
        Task.Factory.StartNew(() =>
        {
            while (true)
            {
                Work();
                Thread.Sleep(1000);
            }
        });
    }

    internal void Work()
    {
        if (Settings.NetworkOperationStateDetectionEnabled)
            Network.CheckDefaultNetworkInterfaceOperationState();
    }
}