using Bike4Me.Domain.Couriers;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Bike4Me.Infrastructure.Repositories;

public sealed class CourierRepository(Bike4MeContext context) : ICourierRepository
{
    public async Task AddAsync(Courier courier)
    {
        await context.Couriers.AddAsync(courier);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj)
        => await context.Couriers.AnyAsync(m => m.Cnpj.Value == cnpj);

    public async Task<Courier?> GetAsync(Guid id)
    {
        return await context.Couriers.FirstOrDefaultAsync(c => c.Id == id);
    }
}