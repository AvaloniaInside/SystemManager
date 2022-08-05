namespace AvaloniaInside;

internal static class Worker
{
    static Worker()
    {
        Task.Factory.StartNew(() =>
        {
            if (Settings.NetworkOperationStateDetectionEnabled)
                Network.CheckDefaultNetworkInterfaceOperationState();

            if (Settings.CpuUsageWatcherEnabled)
                Cpu.UpdateCpuUsageInformation();

            Thread.Sleep(1000);
        });
    }
}