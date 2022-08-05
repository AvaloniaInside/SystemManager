namespace AvaloniaInside;

public class MemoryUsageStateChangedEventArgs : EventArgs
{
    public MemoryUsageState NewState { get; }
    public MemoryUsageState OldState { get; }

    public MemoryUsageStateChangedEventArgs(MemoryUsageState newState, MemoryUsageState oldState)
    {
        NewState = newState;
        OldState = oldState;
    }
}