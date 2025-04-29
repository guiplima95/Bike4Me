using SharedKernel;

namespace Bike4Me.Domain.Motorcycles;
public sealed record Plate
{
    public Plate(string? value)
    {
        Ensure.NotNullOrEmpty(value);
        Value = value.Trim().ToUpper();
    }

    public string Value { get; }
}