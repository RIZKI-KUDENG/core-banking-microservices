using Domain.Common;
namespace Domain.ValueObjects;

public record Money
{
    public decimal Value {get; }
    private Money(decimal value)
    {
        Value = value;
    }
    public static Result<Money> Create(decimal value)
    {
        if(value < 0)
        return Result<Money>.Failure("Amount cannot be negative.");

        return Result<Money>.Success(new Money(value));
    }
    public static Money operator +(Money a, Money b) => new Money(a.Value + b.Value);
    public static Money operator -(Money a, Money b) => new Money(a.Value - b.Value);
}