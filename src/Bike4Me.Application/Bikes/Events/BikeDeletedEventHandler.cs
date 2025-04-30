using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikeDeletedEventHandler(IBikeReportRepository motorcycleReportRepository)
    : INotificationHandler<BikeDeletedEvent>
{
    public async Task Handle(BikeDeletedEvent notification, CancellationToken cancellationToken)
    {
        await motorcycleReportRepository.DeleteAsync(notification.Id.ToString());
    }
}