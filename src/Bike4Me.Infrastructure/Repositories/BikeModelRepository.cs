using Bike4Me.Domain.Bikes;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.Infrastructure.Repositories;

public class BikeModelRepository(Bike4MeContext context) : IBikeModelRepository
{
    public async Task<BikeModel?> GetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string engineCapacity)
    {
        return await context.BikeModels
            .FirstOrDefaultAsync(m =>
                m.Name == name &&
                m.Manufacturer == manufacturer &&
                m.Year == year &&
                m.EngineCapacity == engineCapacity);
    }

    public async Task AddAsync(BikeModel model)
    {
        await context.BikeModels.AddAsync(model);
        await context.SaveChangesAsync();
    }
}