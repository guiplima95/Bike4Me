using Bike4Me.Domain.Rentals;

namespace Bike4Me.Application.Rentals.Dtos;

public record RentalResponse(
    Guid Id,
    Guid BikeId,
    Guid CourierId,
    DateTime RentalStartDate,
    DateTime RentalEndDate,
    DateTime ExpectedReturnDate,
    DateTime? ActualReturnDate,
    RentalStatus Status,
    decimal? TotalPrice)
{
    public RentalResponse() : this(Guid.Empty, Guid.Empty, Guid.Empty, default, default, default, null, RentalStatus.Active, null) { }
}