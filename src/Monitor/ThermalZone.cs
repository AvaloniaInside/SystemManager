namespace AvaloniaInside.SystemManager.Monitor;

public class ThermalZone : IntervalCollection<int>
{
    private readonly string _path;

    internal ThermalZone(string path)
    {
        _path = path;
    }

    protected override async Task<int> NewValueAsync(CancellationToken cancellationToken) =>
        int.Parse((await File.ReadAllTextAsync(_path, cancellationToken)).Trim()) / 100;
    
    public Task<string> GetTypeAsync(CancellationToken cancellationToken) =>
        File.ReadAllTextAsync(_path, cancellationToken);
}