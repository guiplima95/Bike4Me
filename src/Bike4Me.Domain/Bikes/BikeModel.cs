using SharedKernel;

namespace Bike4Me.Domain.Bikes;

public class BikeModel : Entity
{
    // EF Core constructor
    protected BikeModel()
    {
    }

    private BikeModel(Guid id, Name name, Manufacturer manufacturer, Year year, string engineCapacity)
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

    public static BikeModel Create(
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