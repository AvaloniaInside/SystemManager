using System.Runtime.CompilerServices;

namespace AvaloniaInside.SystemManager.Monitor;

public class Thermal
{
    private IReadOnlyList<ThermalZone>? _thermalZones;
    private IDictionary<string, ThermalZone>? _termalZonesByType;

    public IReadOnlyList<ThermalZone> GetZones() =>
        _thermalZones ??= Directory
            .GetDirectories("/sys/class/thermal/", "thermal_zone*")
            .Select(s => new ThermalZone(s))
            .ToList()
            .AsReadOnly();

    public async ValueTask<IDictionary<string, ThermalZone>> GetZoneTypes(CancellationToken cancellationToken)
    {
        if (_termalZonesByType != null)
        {
            return _termalZonesByType;
        }

        _termalZonesByType = new Dictionary<string, ThermalZone>();
        foreach (var zone in GetZones())
        {
            _termalZonesByType[await zone.GetTypeAsync(cancellationToken)] = zone;
        }

        return _termalZonesByType;
    }

    public async ValueTask<ThermalZone> GetByTypeAsync(string name, CancellationToken cancellationToken)
    {
        var zones = await GetZoneTypes(cancellationToken);
        return zones[name];
    }
}