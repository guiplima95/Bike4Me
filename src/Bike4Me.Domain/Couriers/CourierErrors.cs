using SharedKernel;

namespace Bike4Me.Domain.Couriers;

public static class CourierErrors
{
    public static readonly Error DuplicateCnpj = Error.Problem(
        "Courier.DuplicateCnpj", "A courier with this CNPJ already exists.");

    public static readonly Error NotFound = Error.Problem(
        "Courier.NotFound", "Courier not found.");
}