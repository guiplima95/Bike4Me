namespace Bike4Me.Domain.Motorcycles;

public interface IMotorcycleReportRepository
{
    Task UpsertAsync(MotorcycleReport motorcycle);

    Task<MotorcycleReport?> GetByIdAsync(Guid id);

    Task<MotorcycleReport?> GetByPlateAsync(string plate);

    Task<IReadOnlyList<MotorcycleReport>> GetAllAsync();

    Task DeleteAsync(Guid id);
}