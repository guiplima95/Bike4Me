using SharedKernel;

namespace Motorcycle.API.Domain.MotorcycleAggregate;

public class MotorcycleModel : Entity
{
    public Name Name { get; set; } = null!;

    public Manufacturer Manufacturer { get; set; } = null!;

    public Year Year { get; set; } = null!;

    public string Engine { get; set; } = string.Empty;
}