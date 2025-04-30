namespace Bike4Me.Application.Rentals.Commands;

public record ReturnRentalResponse(decimal TotalPrice, string Message = "Invalid Date");