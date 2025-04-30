namespace Bike4Me.Domain.Rentals;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);

    Task UpdateAsync(Rental rental);

    Task<Rental> GetAsync(Guid id);
}