namespace Bike4Me.Application.Rentals.Dtos;

public record RentalResponse(
    Guid Id,
    Guid BikeId,
    Guid CourierId,
    DateTime RentalStartDate,
    DateTime RentalEndDate,
    DateTime ExpectedReturnDate,
    DateTime? ActualReturnDate,
    string Status,
    decimal? TotalPrice)
{
    public RentalResponse() : this(Guid.Empty, Guid.Empty, Guid.Empty, default, default, default, null, string.Empty, null) { }
}