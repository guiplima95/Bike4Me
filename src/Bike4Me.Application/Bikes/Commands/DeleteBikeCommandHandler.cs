using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class DeleteBikeCommandHandler(
    IBikeRepository bikeRepository,
    IMediatorHandler mediator) : IRequestHandler<DeleteBikeCommand, Result>
{
    public async Task<Result> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
    {
        Bike? bike = await bikeRepository.GetAsync(request.Id);
        if (bike is null)
        {
            return Result.Failure(BikeErrors.NotFound);
        }

        await bikeRepository.DeleteAsync(bike);

        await mediator.PublishEvent(new BikeDeletedEvent(bike.Id));

        return Result.Success();
    }
}