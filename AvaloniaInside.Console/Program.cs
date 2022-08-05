using AvaloniaInside;


// *********************************************************************************************************
// *********************************************************************************************************
// network stuff
// *********************************************************************************************************
// *********************************************************************************************************
Settings.DefaultNetworkInterface = "eth0";
Settings.NetworkOperationStateDetectionEnabled = true;

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
Console.WriteLine(
    $"Cpu Usage - Overall {Cpu.OverallUsage.Usage}% Core0: {Cpu.Core0Usage.Usage} Core1: {Cpu.Core1Usage.Usage} Core2: {Cpu.Core2Usage.Usage} Core3: {Cpu.Core3Usage.Usage}");
Console.WriteLine(
    $"Cpu State - Overall {Cpu.OverallUsage.State.ToString()} Core0: {Cpu.Core0Usage.State.ToString()} Core1: {Cpu.Core1Usage.State.ToString()} Core2: {Cpu.Core2Usage.State.ToString()} Core3: {Cpu.Core3Usage.State.ToString()}");





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