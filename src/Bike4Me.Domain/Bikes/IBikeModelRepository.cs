namespace Bike4Me.Domain.Bikes;

public interface IBikeModelRepository
{
    Task<BikeModel?> GetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string engineCapacity,
        CancellationToken cancellationToken = default);

    Task AddAsync(BikeModel model, CancellationToken cancellationToken = default);
}