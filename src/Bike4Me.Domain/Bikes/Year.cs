using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public sealed record Year
{
    public Year(int value)
    {
        Ensure.NotNull(value);
        Ensure.GreaterThanZero(value);

        Value = value;
    }

    public int Value { get; }
}