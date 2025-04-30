using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Bikes.Events;

public class BikeCreatedEventHandler(IBikeReportRepository motorcycleReportRepository)
    : INotificationHandler<BikeCreatedEvent>
{
    public async Task Handle(BikeCreatedEvent notification, CancellationToken cancellationToken)
    {
        await motorcycleReportRepository.UpsertAsync(notification.Bike);
    }
}