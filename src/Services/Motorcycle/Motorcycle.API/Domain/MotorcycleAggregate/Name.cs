using SharedKernel;

namespace Motorcycle.API.Domain.MotorcycleAggregate;

public sealed record Name
{
    public Name(string? value)
    {
        Ensure.NotNullOrEmpty(value);

        Value = value;
    }

    public string Value { get; }
}