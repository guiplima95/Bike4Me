using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public static class BikeErrors
{
    public static readonly Error DuplicatePlate = Error.Conflict(
        "Bikes.NotUnique", "A bike with this plate already exists.");

    public static readonly Error NotFoundByPlate = Error.NotFound(
        "Bikes.NotFoundByPlate",
        "The bike with the specified plate was not found");

    public static readonly Error NotFound = Error.NotFound(
        "Bikes.NotFound",
        "The bike was not found");
}