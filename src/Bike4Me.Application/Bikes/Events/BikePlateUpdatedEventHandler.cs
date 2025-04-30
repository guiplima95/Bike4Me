using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikePlateUpdatedEventHandler(
    IBikeReportRepository reportRepository) : INotificationHandler<BikePlateUpdatedEvent>
{
    public async Task Handle(BikePlateUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIdAsync(notification.BikeId.ToString());
        if (report is null)
        {
            return;
        }

        report.LicensePlate = notification.LicensePlate;

        await reportRepository.UpsertAsync(report);
    }
}