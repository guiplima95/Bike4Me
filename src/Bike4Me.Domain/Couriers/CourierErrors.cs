using SharedKernel;

namespace Bike4Me.Domain.Couriers;

public static class CourierErrors
{
    public static readonly Error DuplicateCnpj = Error.Problem(
        "Courier.DuplicateCnpj", "A courier with this CNPJ already exists.");
}