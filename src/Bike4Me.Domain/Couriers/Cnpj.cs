using SharedKernel;

namespace Bike4Me.Domain.Couriers;

public sealed class Cnpj
{
    public string Value { get; }

    public Cnpj(string value)
    {
        Ensure.NotNullOrEmpty(value);

        Value = RemoveMask(value);
    }

    private static string RemoveMask(string cnpj)
    {
        return new string([.. cnpj.Where(char.IsDigit)]);
    }
}