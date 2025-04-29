using Bike4Me.Domain.Motorcycles;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.Infrastructure.Repositories;

public class ModelMotorcycleRepository(Bike4MeContext context) : IModelMotorcycleRepository
{
    public async Task<MotorcycleModel?> GetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string engineCapacity,
        CancellationToken cancellationToken = default)
    {
        return await context.MotorcycleModels
            .FirstOrDefaultAsync(m =>
                m.Name == name &&
                m.Manufacturer == manufacturer &&
                m.Year == year &&
                m.EngineCapacity == engineCapacity,
                cancellationToken);
    }

    public async Task AddAsync(MotorcycleModel model, CancellationToken cancellationToken = default)
    {
        await context.MotorcycleModels.AddAsync(model, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}