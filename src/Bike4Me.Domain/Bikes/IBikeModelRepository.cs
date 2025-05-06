namespace Bike4Me.Domain.Bikes;

public interface IBikeModelRepository
{
    Task<BikeModel?> GetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string engineCapacity);

    Task AddAsync(BikeModel model);
}