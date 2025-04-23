using Motorcycle.API.Domain.MotorcycleAggregate;

namespace Motorcycle.API.Application.Dtos;

public record MotorcycleDto(string ModelName, string Plate, string Color, MotorcycleStatus Status);