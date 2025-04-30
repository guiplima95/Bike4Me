using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Rentals.Events;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Couriers;
using Bike4Me.Domain.Rentals;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Commands;

public sealed class CreateRentalCommandHandler(
    IBikeRepository bikeRepository,
    ICourierRepository courierRepository,
    IRentalRepository rentalRepository,
    IMediatorHandler mediator) : IRequestHandler<CreateRentalCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        Bike? bike = await bikeRepository.GetAsync(request.MotorcycleId);
        if (bike is null)
        {
            return Result.Failure<Guid>(BikeErrors.NotFound);
        }

        Courier? courier = await courierRepository.GetAsync(request.CourierId);
        if (courier is null)
        {
            return Result.Failure<Guid>(CourierErrors.NotFound);
        }

        if (courier.Cnh.Category != "A")
        {
            return Result.Failure<Guid>(RentalErrors.CourierNotAuthorized);
        }

        if (request.RentalStartDate.Date != DateTime.UtcNow.Date.AddDays(1))
        {
            return Result.Failure<Guid>(RentalErrors.InvalidRentalStartDate);
        }

        if (request.RentalEndDate.Date < request.RentalStartDate.Date)
        {
            return Result.Failure<Guid>(RentalErrors.ExpectedReturnDateBeforeStart);
        }

        if (request.ExpectedReturnDate.Date < request.RentalStartDate.Date)
        {
            return Result.Failure<Guid>(RentalErrors.ExpectedReturnDateBeforeStart);
        }

        RentalPlan? rentalPlan = request.RentalPlanDays switch
        {
            7 => new RentalPlan(7, 30m, 20m, 0m),
            15 => new RentalPlan(15, 28m, 40m, 0m),
            30 => new RentalPlan(30, 22m, 0m, 0m),
            45 => new RentalPlan(45, 20m, 0m, 0m),
            50 => new RentalPlan(50, 18m, 0m, 0m),
            _ => null
        };

        if (rentalPlan is null)
        {
            return Result.Failure<Guid>(RentalErrors.InvalidPlan);
        }

        var rentalResult = Rental.Create(
            request.MotorcycleId,
            request.CourierId,
            rentalPlan,
            request.RentalStartDate);

        if (rentalResult.IsFailure)
        {
            return Result.Failure<Guid>(rentalResult.Error);
        }

        Rental rental = rentalResult.Value;

        var updateExpectedReturnResult = rental.UpdateExpectedReturnDate(request.ExpectedReturnDate);
        if (updateExpectedReturnResult.IsFailure)
        {
            return Result.Failure<Guid>(updateExpectedReturnResult.Error);
        }

        await rentalRepository.AddAsync(rental);

        await mediator.PublishEvent(
           new RentalCreatedEvent(rental.Id, rental.BikeId));

        return Result.Success(rental.Id);
    }
}