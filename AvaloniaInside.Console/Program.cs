using AvaloniaInside;
using AvaloniaInside.Resources;
using AvaloniaInside.SSH;

// *********************************************************************************************************
// network stuff
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
// ssh stuff
//Ssh.StartDaemon();
//Ssh.StopDaemon();
Console.WriteLine($"SSH running: {Ssh.IsRunning}");



// *********************************************************************************************************
// storage stuff
Storage.Test();


// make an endless loop
Task.Factory.StartNew(() =>
{
    while (true) Thread.Sleep(250);
}).Wait();