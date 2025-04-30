using Bike4Me.Application.Abstractions.Messaging.Interfaces;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikePlateUpdatedEvent(Guid bikeId, string licensePlate) : Event
{
    public Guid BikeId => bikeId;
    public string LicensePlate => licensePlate;
}