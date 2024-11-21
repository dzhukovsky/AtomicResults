using AtomicResults.Abstractions;

namespace AtomicResults;
public class Error : IError
{
    public string Message { get; }
    public IReadOnlyList<object?> Metadata { get; }

    public Error(string message, params object?[] metadata)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNull(metadata);

        Message = message;
        // Directly assigns object?[] to IReadOnlyList<object?> to avoid unnecessary .ToArray() call 
        // when passing params to ILogger.Log(..., params object?[] args).
        Metadata = metadata;
    }
}
