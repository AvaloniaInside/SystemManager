namespace SystemManager.Monitor;

public class MemoryUsageInformation
{
    public ulong MemorySize { get; set; }
    public ulong MemoryFree { get; set; }
    public ulong SwapSize { get; set; }
    public ulong SwapFree { get; set; }
    public MemoryUsageState State { get; set; }
}