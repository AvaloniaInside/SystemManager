using AvaloniaInside;

// network stuff
Settings.DefaultNetworkInterface = "eth0";

// listen to OperationState changes for default NetworkInterface
Network.NetworkInterfaceOperationStateChanged += eventargs =>
    Console.WriteLine($"Default NetworkInterface OperationState changed to: {eventargs.NewOperationState}");

// manual get the OperationState from an NetworkInterface
//var operationState = Network.GetNetworkInterfaceOperationState("eth0");

//Network.StartNetworkDaemon();
//Network.StopNetworkDaemon();

// make an endless loop
Task.Factory.StartNew(() =>
{
    while (true) Thread.Sleep(250);
}).Wait();