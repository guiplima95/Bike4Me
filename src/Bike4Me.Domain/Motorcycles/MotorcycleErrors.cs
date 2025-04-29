using SharedKernel;

namespace Bike4Me.Domain.Motorcycles;

public static class MotorcycleErrors
{
    public static readonly Error DuplicatePlate = Error.Conflict(
        "Motorcycles.MotorcycleNotUnique", "A motorcycle with this plate already exists.");

    public static readonly Error NotFoundByPlate = Error.NotFound(
    "Motorcycles.NotFoundByPlate",
    "The motorcycle with the specified plate was not found");

    public static readonly Error NotFound = Error.NotFound(
    "Motorcycles.NotFound",
    "The motorcycle was not found");
}