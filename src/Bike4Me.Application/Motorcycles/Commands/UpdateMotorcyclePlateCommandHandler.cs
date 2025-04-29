using Bike4Me.Application.Abstractions.Messaging;
using Bike4Me.Application.Motorcycles.Events;
using Bike4Me.Domain.Motorcycles;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class UpdateMotorcyclePlateCommandHandler(
    IMotorcycleRepository motorcycleRepository,
    IApplicationEventBus applicationEventBus) : IRequestHandler<UpdateMotorcyclePlateCommand, Result>
{
    public async Task<Result> Handle(UpdateMotorcyclePlateCommand request, CancellationToken cancellationToken)
    {
        if (await motorcycleRepository.AnyExistsAsync(request.Plate))
        {
            return Result.Failure(MotorcycleErrors.DuplicatePlate);
        }

        Motorcycle? motorcycle = await motorcycleRepository.GetAsync(request.Id);
        if (motorcycle is null)
        {
            return Result.Failure(MotorcycleErrors.NotFound);
        }

        Plate plate = new(request.Plate);

        motorcycle.UpdatePlate(plate);

        await motorcycleRepository.UpdateAsync(motorcycle);

        await applicationEventBus.PublishMessage(
            new MotorcyclePlateUpdatedDomainEvent(motorcycle.Id, motorcycle.Plate.Value));

        return Result.Success();
    }
}