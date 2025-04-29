using SharedKernel;

namespace Bike4Me.Domain.Motorcycles;

public class MotorcycleModel : Entity
{
    // EF Core constructor
    protected MotorcycleModel()
    {
    }

    private MotorcycleModel(Guid id, Name name, Manufacturer manufacturer, Year year, string engineCapacity)
    {
        Id = id;
        Name = name;
        Manufacturer = manufacturer;
        Year = year;
        EngineCapacity = engineCapacity;
    }

    public Name Name { get; set; } = null!;

    public Manufacturer Manufacturer { get; set; } = null!;

    public Year Year { get; set; } = null!;

    public string EngineCapacity { get; set; } = string.Empty;

    public static MotorcycleModel Create(
        Guid id,
        Name name,
        Manufacturer
        manufacturer,
        Year year,
        string engineCapacity)
    {
        return new(id, name, manufacturer, year, engineCapacity);
    }
}