using Bike4Me.Domain.Bikes;
using MediatR;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikePlateUpdatedEventHandler(
    IBikeReportRepository bikeReportRepository) : INotificationHandler<BikePlateUpdatedEvent>
{
    public async Task Handle(BikePlateUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var report = await bikeReportRepository.GetByIdAsync(notification.BikeId.ToString());
        if (report is null)
        {
            return;
        }

        report.LicensePlate = notification.LicensePlate;

        await bikeReportRepository.UpsertAsync(report);
    }
}