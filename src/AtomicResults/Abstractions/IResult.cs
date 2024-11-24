using System.Diagnostics.CodeAnalysis;

namespace AtomicResults.Abstractions;
public interface IResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    bool IsFailure { get; }

    IError? Error { get; }
}
