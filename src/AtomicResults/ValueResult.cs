using AtomicResults.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace AtomicResults;
public readonly struct ValueResult : IResult
{
    public IError? Error { get; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error == default;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => Error != default;

    public ValueResult() { }
    public ValueResult(IError error)
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

    public static ValueResult Ok() => default;
    public static ValueResult Fail(IError error) => new(error);

    public static ValueResult<TValue> Ok<TValue>(TValue value) => new(value);
    public static ValueResult<TValue> Fail<TValue>(IError error) => new(error);

    public static implicit operator ValueResult(Error error) => new(error);
}

public readonly struct ValueResult<TValue> : IResult<TValue>
{
    private readonly TValue _value = default!;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException($"Result is in failed state. Value is not set.");

    public IError? Error { get; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error == default;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => Error != default;

    public ValueResult(TValue value)
    {
        _value = value;
    }

    public ValueResult(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        Error = error;
    }

    public ValueResult<TValue> FailIfNull(IError error) => IsSuccess ? this : new(error);

    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess;
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

    public static implicit operator ValueResult<TValue>(TValue value) => new(value);
    public static implicit operator ValueResult<TValue>(Error error) => new(error);
    public static implicit operator ValueResult(ValueResult<TValue> result)
        => result.IsSuccess ? default : new(result.Error);
}