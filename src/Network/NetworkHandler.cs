using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AvaloniaInside.SystemManager;

internal static class NetworkHandler
{
    /// <summary>
    ///     Read all network informations
    /// </summary>
    /// <param name="networkInterface"></param>
    /// <returns></returns>
    internal static NetworkInformation? GetNetworkInformation(string networkInterface)
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(w => w.Name == networkInterface);
        return (from ni in interfaces
            let ipAddresses =
                ni.GetIPProperties().UnicastAddresses.Where(w => w.Address.AddressFamily == AddressFamily.InterNetwork)
                    .ToList()
            let gateways = ni.GetIPProperties().GatewayAddresses.Select(s => s.Address).ToList()
            select new NetworkInformation(GetHostName(), GetIpConfiguration(networkInterface),
                ni.GetPhysicalAddress().ToString(), ipAddresses.Count > 0 ? ipAddresses[0].Address.ToString() : "", "",
                gateways.Select(i => i.ToString()).ToArray())).FirstOrDefault();
    }

    internal static IpConfiguration GetIpConfiguration(string networkInterface)
    {
        foreach (var line in File.ReadLines(SystemConstants.NetworkInterfaces))
            if (line.Contains(networkInterface))
            {
                if (line.Contains("dhcp"))
                    return IpConfiguration.Dhcp;
                if (line.Contains("static"))
                    return IpConfiguration.Static;
            }

        return IpConfiguration.Unknown;
    }

    /// <summary>
    ///     Sets the hostname
    /// </summary>
    /// <param name="hostName"></param>
    internal static void SetHostName(string hostName)
    {
        File.WriteAllText(SystemConstants.HostName, hostName);
    }

    /// <summary>
    ///     Gets the hostname
    /// </summary>
    internal static string GetHostName()
    {
        return File.ReadAllText(SystemConstants.HostName);
    }
}