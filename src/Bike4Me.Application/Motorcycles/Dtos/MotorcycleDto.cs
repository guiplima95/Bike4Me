namespace Bike4Me.Application.Motorcycles.Dtos;

public record MotorcycleDto(
    Guid Id,
    string ModelName,
    string Plate,
    int Year);