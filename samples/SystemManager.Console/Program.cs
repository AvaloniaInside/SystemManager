using AvaloniaInside.SystemManager;
using AvaloniaInside.SystemManager.Helpers;
using AvaloniaInside.SystemManager.Monitor;

var cts = new CancellationTokenSource();

AvaloniaInside.SystemManager.System.Init();

Settings.DefaultNetworkInterface = "eth0";
Settings.NetworkOperationStateDetectionEnabled = true;
Settings.CpuUsageWatcherEnabled = true;
// Settings.MemoryUsageWatcherEnabled = true;
// Settings.MemoryUsageWarningLevel = 60;
// Settings.MemoryUsageOverloadLevel = 80;

// *********************************************************************************************************
// *********************************************************************************************************
// network stuff
// *********************************************************************************************************
// *********************************************************************************************************
var networkInformation = Network.GetNetworkInformation(Settings.DefaultNetworkInterface);

Console.WriteLine($"DefaultNetworkInterface OperationState: {Network.DefaultInterfaceOperationState.ToString()}");

// listen to OperationState changes for default NetworkInterface
Network.NetworkInterfaceOperationStateChanged += eventargs =>
    Console.WriteLine($"Default NetworkInterface OperationState changed to: {eventargs.NewOperationState}");

// manual get the OperationState from an NetworkInterface
//var operationState = Network.GetNetworkInterfaceOperationState("eth0");

//Network.StartDaemon();
//Network.StopDaemon();


// *********************************************************************************************************
// *********************************************************************************************************
// cpu stuff
// *********************************************************************************************************
// *********************************************************************************************************
_ = Task.Factory.StartNew(async () =>
{
    var cpuUsage = new CpuUsage();
    await foreach (var cpu in cpuUsage.WithCancellation(cts.Token))
    {
        Console.WriteLine($"Cpu Usage: {cpu.Usage:0.00}");
        var cores = cpu.Cores.Select(s => s.ToString("0.00")).Aggregate((o, n) => $"{o},{n}");
        Console.WriteLine($"Cores: {cores}");
    }
});

// *********************************************************************************************************
// *********************************************************************************************************
// memory stuff
// *********************************************************************************************************
// *********************************************************************************************************
_ = Task.Factory.StartNew(async () =>
{
    var memoryUsage = new MemoryUsage();
    await foreach (var info in memoryUsage.WithCancellation(cts.Token))
    {
        Console.WriteLine($"Memory: {info.MemoryFree.BytesToString()}:{info.MemorySize.BytesToString()}");
        Console.WriteLine($"Swap: {info.SwapFree.BytesToString()}:{info.SwapSize.BytesToString()}");
        Console.WriteLine($"Memory State: {info.State}");
    }
});

// *********************************************************************************************************
// *********************************************************************************************************
// ssh stuff
// *********************************************************************************************************
// *********************************************************************************************************

//Ssh.StartDaemon();
//Ssh.StopDaemon();
Console.WriteLine($"SSH running: {Ssh.IsRunning}");


// *********************************************************************************************************
// *********************************************************************************************************
// storage stuff
// *********************************************************************************************************
// *********************************************************************************************************
Storage.Test();


// make an endless loop
Task.Factory.StartNew(() =>
{
    while (Console.ReadLine() != "q")
    {
    }

    cts.Cancel();
}).Wait();