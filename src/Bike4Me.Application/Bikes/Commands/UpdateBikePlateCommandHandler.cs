﻿using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class UpdateBikePlateCommandHandler(
    IBikeRepository bikeRepository,
    IMediatorHandler mediator) : IRequestHandler<UpdateBikePlateCommand, Result>
{
    public async Task<Result> Handle(UpdateBikePlateCommand request, CancellationToken cancellationToken)
    {
        if (await bikeRepository.AnyExistsAsync(request.LicensePlate))
        {
            return Result.Failure(BikeErrors.DuplicatePlate);
        }

        Bike? bike = await bikeRepository.GetAsync(request.Id);
        if (bike is null)
        {
            return Result.Failure(BikeErrors.NotFound);
        }

        LicensePlate plate = new(request.LicensePlate);

        bike.UpdatePlate(plate);

        await bikeRepository.UpdateAsync(bike);

        await mediator.PublishEvent(new BikePlateUpdatedEvent(bike.Id, bike.Plate.Value));

        return Result.Success();
    }
}