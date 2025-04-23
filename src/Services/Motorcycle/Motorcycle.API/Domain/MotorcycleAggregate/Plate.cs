using SharedKernel;

namespace Motorcycle.API.Domain.MotorcycleAggregate;

public sealed record Plate
{
    public Plate(string? value)
    {
        Ensure.NotNullOrEmpty(value);
        Value = value.Trim().ToUpper();
    }

    public string Value { get; }
}

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