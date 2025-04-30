namespace Bike4Me.Domain.Bikes;

public interface IBikeReportRepository
{
    Task UpsertAsync(BikeReport bike);

    Task<BikeReport?> GetByIdAsync(Guid id);

    Task<BikeReport?> GetByPlateAsync(string plate);

    Task<IReadOnlyList<BikeReport>> GetAllAsync();

    Task DeleteAsync(Guid id);
}