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
        string engineCapacity,
        CancellationToken cancellationToken = default)
    {
        return await context.BikeModels
            .FirstOrDefaultAsync(m =>
                m.Name == name &&
                m.Manufacturer == manufacturer &&
                m.Year == year &&
                m.EngineCapacity == engineCapacity,
                cancellationToken);
    }

    public async Task AddAsync(BikeModel model, CancellationToken cancellationToken = default)
    {
        await context.BikeModels.AddAsync(model, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}