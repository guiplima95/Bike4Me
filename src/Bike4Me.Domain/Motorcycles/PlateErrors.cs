using SharedKernel;

namespace Bike4Me.Domain.Motorcycles;

public static class PlateErrors
{
    public static readonly Error InvalidFormat = Error.Failure(
        "Plate.PlateInvalidFormat", "Invalid plate format. Expected format: ABC-1234.");
}