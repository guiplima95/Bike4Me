using Bike4Me.Domain.Bikes;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MotorcycleEntity = Bike4Me.Domain.Bikes.Bike;

namespace Bike4Me.Infrastructure.Repositories;

public class BikeRepository(Bike4MeContext context) : IBikeRepository
{
    public async Task AddAsync(MotorcycleEntity bike)
    {
        await context.Bikes.AddAsync(bike);
        await context.SaveChangesAsync();
    }

    public async Task<MotorcycleEntity?> GetAsync(Guid id)
    {
        return await context.Bikes
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MotorcycleEntity>> GetAllAsync()
    {
        return await context.Bikes
            .ToListAsync();
    }

    public async Task UpdateAsync(MotorcycleEntity bike)
    {
        context.Bikes.Update(bike);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MotorcycleEntity bike)
    {
        context.Bikes.Remove(bike);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AnyExistsAsync(string plate)
    {
        return await context.Bikes
            .AnyAsync(m => m.Plate.Value == plate);
    }
}