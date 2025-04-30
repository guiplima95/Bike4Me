namespace Bike4Me.Application.Bikes.Dtos;

public record BikeResponse(
    Guid Id,
    string ModelName,
    string LicensePlate,
    int Year);