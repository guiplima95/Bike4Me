using SharedKernel;

namespace Bike4Me.Domain.Couriers;

public static class CnhErrors
{
    public static readonly Error EmptyNumber = Error.Problem("Cnh.EmptyNumber", "CNH number is empty");

    public static readonly Error EmptyCategory = Error.Problem("Cnh.EmptyCategory", "CNH category is empty");

    public static readonly Error InvalidNumberFormat = Error.Problem(
        "Cnh.InvalidNumberFormat", "CNH number format is invalid. It must contain exactly 11 digits.");

    public static readonly Error InvalidCategory = Error.Problem(
        "Cnh.InvalidCategory", "CNH category is invalid. Valid categories are: A, B, A+B.");
}