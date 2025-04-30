using SharedKernel;

namespace Bike4Me.Domain.Bikes;
public sealed record LicensePlate
{
    public LicensePlate(string? value)
    {
        Ensure.NotNullOrEmpty(value);
        Value = value.Trim().ToUpper();
    }

    public string Value { get; }
}