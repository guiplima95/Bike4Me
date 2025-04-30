namespace Bike4Me.Domain.Bikes;

public interface IBikeReportRepository
{
    Task UpsertAsync(BikeReport bike);

    Task DeleteAsync(string id);

    Task<BikeReport?> GetByIdAsync(string id);
}