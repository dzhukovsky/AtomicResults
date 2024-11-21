using AtomicResults.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace AtomicResults;
public class Result : IResult
{
    private static readonly Result ResultOk = new();

    public IError? Error { get; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error == default;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => Error != default;

    public Result() { }
    public Result(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        Error = error;
    }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasError<TError>() where TError : IError
        => IsFailure && Error is TError;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool TryGetError([NotNullWhen(true)] out IError? error)
        => (error = Error) != default;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool TryGetError<TError>([NotNullWhen(true)] out TError? error)
        where TError : IError
    {
        if (Error is TError err)
        {
            error = err;
            return true;
        }

        error = default;
        return false;
    }

    public static Result Ok() => ResultOk;
    public static Result Fail(IError error) => new(error);

    public static Result<TValue> Ok<TValue>(TValue value) => new(value);
    public static Result<TValue> Fail<TValue>(IError error) => new(error);

    public static implicit operator Result(Error error) => new(error);
}

public class Result<TValue> : Result, IResult<TValue>
{
    private readonly TValue _value = default!;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException($"Result is in failed state. Value is not set.");

    public Result(TValue value)
    {
        _value = value;
    }

    public Result(IError error) : base(error)
    {
    }

    public Result<TValue> FailIfNull(IError error) 
        => IsSuccess && Value == null ? new(error) : this;

    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}