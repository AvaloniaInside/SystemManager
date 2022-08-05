namespace AvaloniaInside;

internal class Worker
{
    internal void Start()
    {
        Task.Factory.StartNew(() =>
        {
            while (true)
            {
                if (Settings.NetworkOperationStateDetectionEnabled)
                    Network.CheckDefaultNetworkInterfaceOperationState();

                if (Settings.CpuUsageWatcherEnabled)
                    Cpu.UpdateCpuUsageInformation();
                
                if (Settings.MemoryUsageWatcherEnabled)
                    Memory.UpdateMemoryUsageInformation();

                Thread.Sleep(1000);
            }
        });
    }
}