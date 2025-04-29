using Bike4Me.Domain.Motorcycles;
using MediatR;

namespace Bike4Me.Application.Motorcycles.Events;

public class MotorcycleCreatedDomainEventHandler(IMotorcycleReportRepository motorcycleReportRepository)
    : INotificationHandler<MotorcycleCreatedDomainEvent>
{
    public async Task Handle(MotorcycleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await motorcycleReportRepository.UpsertAsync(notification.Motorcycle);
    }
}