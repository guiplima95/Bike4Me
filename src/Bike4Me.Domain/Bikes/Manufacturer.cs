using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public sealed record Manufacturer
{
    public Manufacturer(string? value)
    {
        Ensure.NotNullOrEmpty(value);

        Value = value;
    }

    public string Value { get; }
}