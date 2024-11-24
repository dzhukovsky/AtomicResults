namespace AtomicResults.Abstractions;
public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}