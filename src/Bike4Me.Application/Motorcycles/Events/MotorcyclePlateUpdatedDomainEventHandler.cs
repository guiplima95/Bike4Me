using Bike4Me.Domain.Motorcycles;
using MediatR;

namespace Bike4Me.Application.Motorcycles.Events;

public sealed class MotorcyclePlateUpdatedDomainEventHandler(IMotorcycleReportRepository reportRepository)
    : INotificationHandler<MotorcyclePlateUpdatedDomainEvent>
{
    public async Task Handle(MotorcyclePlateUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIdAsync(notification.MotorcycleId);
        if (report is null)
        {
            return;
        }

        report.Plate = notification.Plate;

        await reportRepository.UpsertAsync(report);
    }
}