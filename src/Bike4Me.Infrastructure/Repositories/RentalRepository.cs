using Bike4Me.Domain.Rentals;

namespace Bike4Me.Infrastructure.Repositories;

public sealed class RentalRepository : IRentalRepository
{
    public Task AddAsync(Rental rental)
    {
        throw new NotImplementedException();
    }

    public Task<Rental> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Rental rental)
    {
        throw new NotImplementedException();
    }
}