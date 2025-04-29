namespace Bike4Me.Domain.Motorcycles;

public interface IModelMotorcycleRepository
{
    Task<MotorcycleModel?> GetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string engineCapacity,
        CancellationToken cancellationToken = default);

    Task AddAsync(MotorcycleModel model, CancellationToken cancellationToken = default);
}