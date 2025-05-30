﻿using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public class Bike : Entity
{
    // EF Core constructor
    protected Bike()
    {
    }

    private Bike(
        Guid id,
        LicensePlate plate,
        Guid modelId,
        BikeStatus status,
        string color)
    {
        Id = id;
        Plate = plate;
        ModelId = modelId;
        Status = status;
        Color = color;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public LicensePlate Plate { get; set; } = null!;

    public Guid ModelId { get; set; }

    public string Color { get; set; } = string.Empty;

    public BikeStatus? Status { get; set; }

    public static Bike Create(
        Guid id,
        LicensePlate plate,
        Guid modelId,
        string color)
    {
        Bike bike = new(id, plate, modelId, BikeStatus.Available, color);

        return bike;
    }

    public void UpdatePlate(LicensePlate plate)
    {
        Plate = plate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsRented() => Status = BikeStatus.Rented;

    public void MarkAsAvailable() => Status = BikeStatus.Available;

    public static BikeReport Build(Guid id, string plate, string modelName, int year)
    {
        return new BikeReport
        {
            Id = id.ToString(),
            LicensePlate = plate,
            ModelName = modelName,
            Year = year
        };
    }
}