using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Bikes.Events;

public class BikeCreatedEventHandler(IBikeReportRepository bikeReportRepository)
    : INotificationHandler<BikeCreatedEvent>
{
    public async Task Handle(BikeCreatedEvent notification, CancellationToken cancellationToken)
    {
        await bikeReportRepository.UpsertAsync(notification.Bike);
    }
}