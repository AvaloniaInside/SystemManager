namespace AvaloniaInside.SystemManager.Monitor;

public class ThermalZone : IntervalCollection<float>
{
    private readonly string _path;

    internal ThermalZone(string path)
    {
        _path = path;
    }

    protected override async Task<float> NewValueAsync(CancellationToken cancellationToken) =>
        float.Parse((await File.ReadAllTextAsync(_path, cancellationToken)).Trim()) / 100f;
    
    public Task<string> GetTypeAsync(CancellationToken cancellationToken) =>
        File.ReadAllTextAsync(_path, cancellationToken);
}