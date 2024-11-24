using AtomicResults.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace AtomicResults;
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