namespace AvaloniaInside.SystemManager.Monitor;

public abstract class IntervalCollection<T>: IAsyncEnumerable<T>, IAsyncEnumerator<T>
{
    private bool _isStarted;
    private bool _isDisposed;
    private CancellationToken _cancellationToken;

    public virtual T Current { get; protected set; }
    
    /// <summary>
    /// Interval of monitor check in milliseconds, recommended to use default value, 1000 
    /// </summary>
    public virtual int Interval { get; set; } = 1000;

    protected abstract Task<T> NewValueAsync(CancellationToken cancellationToken);
    
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        if (_isDisposed) throw new ObjectDisposedException("Object already disposed");
        
        _cancellationToken = cancellationToken;
        if (_isStarted) throw new InvalidOperationException("Already is running. Create another instance");
        _isStarted = true;

        return this;
    }
    
    public virtual ValueTask DisposeAsync()
    {
        if (_isDisposed) return ValueTask.CompletedTask;
        Disposing();
        return ValueTask.CompletedTask;
    }

    protected virtual void Disposing()
    {
        GC.SuppressFinalize(this);
        _isDisposed = true;
        Current = default;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        if (_isDisposed) throw new ObjectDisposedException("Object already disposed");
        if (!_isStarted) throw new InvalidOperationException("Bad usage, Please use async foreach to start operations");

        await Task.Delay(Interval, _cancellationToken).ConfigureAwait(false);
        Current = await NewValueAsync(_cancellationToken);

        return true;
    }
}