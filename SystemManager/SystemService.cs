namespace SystemManager;

internal static class SystemService
{
    internal static void Start(string service)
    {
        Call(service, "start");
    }

    internal static void Stop(string service)
    {
        Call(service, "stop");
    }

    private static void Call(string service, string parameter)
    {
        try
        {
            Bash.Execute($"/etc/init.d/{service}", parameter, out var error, out _);
            if (!string.IsNullOrEmpty(error)) Console.WriteLine($"Error: {error}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}