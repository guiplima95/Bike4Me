using SharedKernel;

namespace Motorcycle.API.Domain.MotorcycleAggregate;

public class Motorcycle : Entity
{
    public Plate Plate { get; set; } = null!;

    public Guid ModelId { get; set; }

    public string Color { get; set; } = string.Empty;

    public MotorcycleStatus Status { get; set; }
}