namespace AtomicResults.Abstractions;
public interface IError
{
    string Message { get; }
    public IReadOnlyList<object?> Metadata { get; }
}
