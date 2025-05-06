using Bike4Me.Domain.Rentals;
using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.Infrastructure.Repositories;

public sealed class RentalRepository(Bike4MeContext context) : IRentalRepository
{
    public async Task AddAsync(Rental rental)
    {
        await context.AddAsync(rental);
        await context.SaveChangesAsync();
    }

    public async Task<Rental?> GetAsync(Guid id)
    {
        return await context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Rental rental)
    {
        context.Update(rental);
        await context.SaveChangesAsync();
    }
}