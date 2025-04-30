namespace Bike4Me.Domain.Bikes;

public interface IBikeRepository
{
    Task AddAsync(Bike bike);

    Task<Bike?> GetAsync(Guid id);

    Task<bool> AnyExistsAsync(string plate);

    Task<IEnumerable<Bike>> GetAllAsync();

    Task UpdateAsync(Bike bike);

    Task DeleteAsync(Bike bike);
}