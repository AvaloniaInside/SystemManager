namespace AvaloniaInside;

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

        if (Settings.CpuUsageWatcherEnabled)
            Cpu.UpdateCpuUsageInformation();
                
        if (Settings.MemoryUsageWatcherEnabled)
            Memory.UpdateMemoryUsageInformation();
    }
}