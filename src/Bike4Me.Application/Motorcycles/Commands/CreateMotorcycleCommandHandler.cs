using Bike4Me.Application.Abstractions.Messaging;
using Bike4Me.Application.Motorcycles.Events;
using Bike4Me.Domain.Motorcycles;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class CreateMotorcycleCommandHandler(
    IModelMotorcycleRepository modelRepository,
    IMotorcycleRepository motorcycleRepository,
    IApplicationEventBus applicationEventBus) : IRequestHandler<CreateMotorcycleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        Plate plate = new(request.Plate);
        Manufacturer manufacturer = new(request.ModelManufacturer);
        Year year = new(request.ModelYear);
        Name modelName = new(request.ModelName);

        MotorcycleModel model = await EnsureMotorcycleModelAsync(
            request.ModelEngineCapacity,
            manufacturer,
            year,
            modelName,
            cancellationToken);

        if (await motorcycleRepository.AnyExistsAsync(request.Plate))
        {
            return Result.Failure<Guid>(MotorcycleErrors.DuplicatePlate);
        }

        Motorcycle motorcycle = Motorcycle.Create(
            Guid.NewGuid(),
            plate,
            model.Id,
            request.Color);

        await motorcycleRepository.AddAsync(motorcycle);

        MotorcycleReport motorcycleReport = Motorcycle.Build(
            motorcycle.Id, motorcycle.Plate.Value, model.Name.Value, model.Year.Value);

        await applicationEventBus.PublishMessage(
            new MotorcycleCreatedDomainEvent(motorcycleReport));

        return Result.Success(motorcycle.Id);
    }

    private async Task<MotorcycleModel> EnsureMotorcycleModelAsync(
        string modelEngineCapacity,
        Manufacturer manufacturer,
        Year year,
        Name modelName,
        CancellationToken cancellationToken)
    {
        MotorcycleModel model;

        MotorcycleModel? existingModel = await modelRepository.GetModelAsync(
            modelName,
            manufacturer,
            year,
            modelEngineCapacity,
            cancellationToken);

        if (existingModel is null)
        {
            model = MotorcycleModel.Create(
                 Guid.NewGuid(),
                 modelName,
                 manufacturer,
                 year,
                 modelEngineCapacity);

            await modelRepository.AddAsync(model, cancellationToken);
            return model;
        }

        return existingModel;
    }
}