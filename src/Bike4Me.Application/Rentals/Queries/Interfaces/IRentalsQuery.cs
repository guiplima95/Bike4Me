using Bike4Me.Application.Rentals.Dtos;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Queries.Interfaces;

public interface IRentalsQuery
{
    Task<Result<RentalResponse>> FindAByIdAsync(Guid id);
}