using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Commands;

public record ReturnRentalCommand(
    Guid RentalId,
    DateTime ActualReturnDate) : IRequest<Result<ReturnRentalResponse>>;