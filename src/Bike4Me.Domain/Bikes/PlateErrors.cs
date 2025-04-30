using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public static class PlateErrors
{
    public static readonly Error InvalidFormat = Error.Failure(
        "LicensePlate.PlateInvalidFormat", "Invalid plate format. Expected format: ABC-1234.");
}