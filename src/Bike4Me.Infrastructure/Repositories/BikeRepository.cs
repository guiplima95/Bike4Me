using Bike4Me.Domain.Bikes;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.Infrastructure.Repositories;

public class BikeRepository(Bike4MeContext context) : IBikeRepository
{
    public async Task AddAsync(Bike bike)
    {
        await context.Bikes.AddAsync(bike);
        await context.SaveChangesAsync();
    }

    public async Task<Bike?> GetAsync(Guid id)
    {
        return await context.Bikes
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task UpdateAsync(Bike bike)
    {
        context.Bikes.Update(bike);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Bike bike)
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