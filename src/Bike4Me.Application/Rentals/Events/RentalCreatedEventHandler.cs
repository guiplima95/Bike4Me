using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Rentals.Events;

public class RentalCreatedEventHandler(IBikeRepository bikeRepository) : INotificationHandler<RentalCreatedEvent>
{
    public async Task Handle(RentalCreatedEvent notification, CancellationToken cancellationToken)
    {
        Bike? bike = await bikeRepository.GetAsync(notification.BikeId);
        if (bike is null)
        {
            return;
        }

        bike.MarkAsRented();

        await bikeRepository.UpdateAsync(bike);
    }
}