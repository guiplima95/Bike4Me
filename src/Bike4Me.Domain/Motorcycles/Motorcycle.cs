using SharedKernel;

namespace Bike4Me.Domain.Motorcycles;

public class Motorcycle : Entity
{
    // EF Core constructor
    protected Motorcycle()
    {
    }

    private Motorcycle(Guid id, Plate plate, Guid modelId, MotorcycleStatus status, string color)
    {
        Id = id;
        Plate = plate;
        ModelId = modelId;
        Status = status;
        Color = color;
    }

    public Plate Plate { get; set; } = null!;

    public Guid ModelId { get; set; }

    public string Color { get; set; } = string.Empty;

    public MotorcycleStatus? Status { get; set; }

    public static Motorcycle Create(
        Guid id,
        Plate plate,
        Guid modelId,
        string color)
    {
        Motorcycle motorcycle = new(id, plate, modelId, MotorcycleStatus.Available, color);

        return motorcycle;
    }

    public void UpdatePlate(Plate plate)
    {
        Plate = plate;
    }

    public static MotorcycleReport Build(Guid id, string plate, string modelName, int year)
    {
        return new MotorcycleReport
        {
            Id = id.ToString(),
            Plate = plate,
            ModelName = modelName,
            Year = year
        };
    }
}