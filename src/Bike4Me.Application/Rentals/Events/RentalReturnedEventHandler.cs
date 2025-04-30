using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Rentals.Events;

public class RentalReturnedEventHandler : INotificationHandler<RentalReturnedEvent>
{
    private readonly IBikeRepository bikeRepository;

    public RentalReturnedEventHandler(IBikeRepository bikeRepository)
    {
        this.bikeRepository = bikeRepository;
    }

    public async Task Handle(RentalReturnedEvent notification, CancellationToken cancellationToken)
    {
        Bike? bike = await bikeRepository.GetAsync(notification.BikeId);
        if (bike is null)
        {
            return;
        }

        bike.MarkAsAvailable();

        await bikeRepository.UpdateAsync(bike);
    }
}