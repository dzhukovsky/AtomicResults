using AtomicResults.Abstractions;

namespace AtomicResults.Tests.Errors;
public readonly struct StructError(string message, params object[] metadata) : IError
{
    public string Message { get; } = message;
    public IReadOnlyList<object?> Metadata { get; } = metadata;
}