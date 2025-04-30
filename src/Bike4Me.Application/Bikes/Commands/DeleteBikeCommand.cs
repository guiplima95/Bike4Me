using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public record DeleteBikeCommand(Guid Id) : IRequest<Result>;

public sealed class DeleteMotorcycleCommandHandler(
    IBikeRepository motorcycleRepository,
    IApplicationEventBus applicationEventBus) : IRequestHandler<DeleteBikeCommand, Result>
{
    public async Task<Result> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
    {
        Bike? bike = await motorcycleRepository.GetAsync(request.Id);
        if (bike is null)
        {
            return Result.Failure(BikeErrors.NotFound);
        }

        await motorcycleRepository.DeleteAsync(bike);

        await applicationEventBus.PublishMessage(new BikeDeletedEvent(request.Id));

        return Result.Success();
    }
}