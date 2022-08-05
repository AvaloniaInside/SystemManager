using AvaloniaInside;

AvaloniaInside.System.Init();

// *********************************************************************************************************
// *********************************************************************************************************
// network stuff
// *********************************************************************************************************
// *********************************************************************************************************
Settings.DefaultNetworkInterface = "eth0";
Settings.NetworkOperationStateDetectionEnabled = true;
Settings.CpuUsageWatcherEnabled = true;
Settings.MemoryUsageWatcherEnabled = true;
Settings.MemoryUsageWarningLevel = 60;
Settings.MemoryUsageOverloadLevel = 80;

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
void PrintCpuUsage()
{
    Console.WriteLine(
        $"Cpu Usage - Overall {Cpu.OverallUsage.Usage}% Core0: {Cpu.Core0Usage.Usage}% Core1: {Cpu.Core1Usage.Usage}% Core2: {Cpu.Core2Usage.Usage}% Core3: {Cpu.Core3Usage.Usage}%");
}

void PrintCpuUsageState()
{
    Console.WriteLine(
        $"Cpu State - Overall {Cpu.OverallUsage.State.ToString()} Core0: {Cpu.Core0Usage.State.ToString()} Core1: {Cpu.Core1Usage.State.ToString()} Core2: {Cpu.Core2Usage.State.ToString()} Core3: {Cpu.Core3Usage.State.ToString()}");
}

PrintCpuUsage();
PrintCpuUsageState();
Cpu.CpuUsageStateChanged += eventArgs =>
{
    Console.WriteLine("CpuUsageStateChanged:");
    PrintCpuUsageState();
};

// *********************************************************************************************************
// *********************************************************************************************************
// memory stuff
// *********************************************************************************************************
// *********************************************************************************************************
void PrintMemory()
{
    Console.WriteLine(
        $"Memory Total {Memory.Total} Free: {Memory.Free} Usage: {Memory.Usage}% State: {Memory.State.ToString()}");
}

PrintMemory();
Memory.MemoryUsageStateChanged += eventArgs =>
{
    Console.WriteLine("MemoryUsageStateChanged:");
    PrintMemory();
};

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
    while (true) Thread.Sleep(250);
}).Wait();