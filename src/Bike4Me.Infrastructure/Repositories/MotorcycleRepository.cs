using Bike4Me.Domain.Motorcycles;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MotorcycleEntity = Bike4Me.Domain.Motorcycles.Motorcycle;

namespace Bike4Me.Infrastructure.Repositories;

public class MotorcycleRepository(Bike4MeContext context) : IMotorcycleRepository
{
    public async Task AddAsync(MotorcycleEntity motorcycle)
    {
        await context.Motorcycles.AddAsync(motorcycle);
        await context.SaveChangesAsync();
    }

    public async Task<MotorcycleEntity?> GetAsync(Guid id)
    {
        return await context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MotorcycleEntity>> GetAllAsync()
    {
        return await context.Motorcycles
            .ToListAsync();
    }

    public async Task UpdateAsync(MotorcycleEntity motorcycle)
    {
        context.Motorcycles.Update(motorcycle);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var motorcycle = await context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id);

        if (motorcycle is null)
        {
            return;
        }

        context.Motorcycles.Remove(motorcycle);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AnyExistsAsync(string plate)
    {
        return await context.Motorcycles
            .AnyAsync(m => m.Plate.Value == plate);
    }
}