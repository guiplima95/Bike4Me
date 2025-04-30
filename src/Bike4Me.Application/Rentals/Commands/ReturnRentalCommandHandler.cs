using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Rentals.Events;
using Bike4Me.Domain.Rentals;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Commands;

public sealed class ReturnRentalCommandHandler(
    IRentalRepository rentalRepository,
    IMediatorHandler mediator) : IRequestHandler<ReturnRentalCommand, Result<ReturnRentalResponse>>
{
    public async Task<Result<ReturnRentalResponse>> Handle(ReturnRentalCommand request, CancellationToken cancellationToken)
    {
        Rental rental = await rentalRepository.GetAsync(request.RentalId);
        if (rental is null)
        {
            return Result.Failure<ReturnRentalResponse>(RentalErrors.NotFound);
        }

        var result = rental.ReturnBike(request.ActualReturnDate);
        if (result.IsFailure)
        {
            return Result.Failure<ReturnRentalResponse>(result.Error);
        }

        await rentalRepository.UpdateAsync(rental);

        await mediator.PublishEvent(
            new RentalCreatedEvent(rental.Id, rental.BikeId));

        return Result.Success(
            new ReturnRentalResponse(rental.TotalPrice ?? 0m, "Return date entered successfully"));
    }
}