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

    public static readonly Error InvalidImageBase64 = Error.Problem(
    "Cnh.InvalidImage", "Invalid base64 string for CNH image.");
}

public static class CnpjErrors
{
    public static readonly Error Empty = Error.Problem("Cnpj.Empty", "CNPJ is empty");

    public static readonly Error InvalidFormat = Error.Problem(
        "Cnpj.InvalidFormat", "CNPJ format is invalid. Expected format: 00.000.000/0000-00");

    public static readonly Error InvalidLength = Error.Problem(
        "Cnpj.InvalidLength", "CNPJ must contain exactly 14 digits (numbers only).");

    public static readonly Error InvalidChecksum = Error.Problem(
        "Cnpj.InvalidChecksum", "CNPJ checksum is invalid.");
}