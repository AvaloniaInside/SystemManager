namespace AvaloniaInside.SystemManager;

public static class System
{
    private static Worker _worker = new();

    public static void Init()
    {
        _worker.Work();
        _worker.StartEndlessLoop();
    }
}