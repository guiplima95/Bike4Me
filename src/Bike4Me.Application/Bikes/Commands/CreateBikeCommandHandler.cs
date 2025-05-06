using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class CreateBikeCommandHandler(
    IBikeModelRepository bikeModelRepository,
    IBikeRepository bikeRepository,
    IMediatorHandler mediator) : IRequestHandler<CreateBikeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
    {
        LicensePlate plate = new(request.Plate);
        Manufacturer manufacturer = new(request.ModelManufacturer);
        Year year = new(request.ModelYear);
        Name modelName = new(request.ModelName);

        BikeModel model = await EnsureBikeModelAsync(
            request.ModelEngineCapacity,
            manufacturer,
            year,
            modelName,
            cancellationToken);

        if (await bikeRepository.AnyExistsAsync(request.Plate))
        {
            return Result.Failure<Guid>(BikeErrors.DuplicatePlate);
        }

        Bike bike = Bike.Create(
            Guid.NewGuid(),
            plate,
            model.Id,
            request.Color);

        await bikeRepository.AddAsync(bike);

        BikeReport bikeReport = Bike.Build(
            bike.Id, bike.Plate.Value, model.Name.Value, model.Year.Value);

        await mediator.PublishEvent(new BikeCreatedEvent(bikeReport));

        return Result.Success(bike.Id);
    }

    private async Task<BikeModel> EnsureBikeModelAsync(
        string modelEngineCapacity,
        Manufacturer manufacturer,
        Year year,
        Name modelName,
        CancellationToken cancellationToken)
    {
        BikeModel model;

        BikeModel? existingModel = await bikeModelRepository.GetModelAsync(
            modelName,
            manufacturer,
            year,
            modelEngineCapacity);

        if (existingModel is null)
        {
            model = BikeModel.Create(
                 Guid.NewGuid(),
                 modelName,
                 manufacturer,
                 year,
                 modelEngineCapacity);

            await bikeModelRepository.AddAsync(model);
            return model;
        }

        return existingModel;
    }
}