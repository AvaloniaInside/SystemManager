using System.Diagnostics;
using AvaloniaInside;
using AvaloniaInside.SSH;

// *********************************************************************************************************
// network stuff
Settings.DefaultNetworkInterface = "eth0";
Settings.NetworkOperationStateDetectionEnabled = true;

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

// make an endless loop
Task.Factory.StartNew(() =>
{
    while (true) Thread.Sleep(250);
}).Wait();