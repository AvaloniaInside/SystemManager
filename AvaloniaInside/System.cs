namespace AvaloniaInside;

public static class System
{
    private static Worker _worker = new();

    public static void Init()
    {
        _worker.Start();
    }
}