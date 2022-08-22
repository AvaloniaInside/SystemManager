namespace AvaloniaInside.SystemManager;

public class NetworkInformation
{
    public NetworkInformation(string hostName, IpConfiguration ipConfiguration, string macAddress, string ipAddress,
        string ipV4Mask,
        string[] gatewaysList)
    {
        HostName = hostName;
        IpConfiguration = ipConfiguration;
        MacAddress = macAddress;
        IpAddress = ipAddress;
        IpV4Mask = ipV4Mask;
        GatewaysList = gatewaysList;
    }

    public string HostName { get; }
    public IpConfiguration IpConfiguration { get; }
    public string MacAddress { get; }
    public string IpAddress { get; }
    public string IpV4Mask { get; }
    public string[] GatewaysList { get; }

    public string Gateways => string.Join(",", GatewaysList);
}