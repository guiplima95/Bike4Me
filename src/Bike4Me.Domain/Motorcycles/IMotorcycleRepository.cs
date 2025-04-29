namespace Bike4Me.Domain.Motorcycles;

public interface IMotorcycleRepository
{
    Task AddAsync(Motorcycle motorcycle);

    Task<Motorcycle?> GetAsync(Guid id);

    Task<bool> AnyExistsAsync(string plate);

    Task<IEnumerable<Motorcycle>> GetAllAsync();

    Task UpdateAsync(Motorcycle motorcycle);

    Task DeleteAsync(Guid id);
}