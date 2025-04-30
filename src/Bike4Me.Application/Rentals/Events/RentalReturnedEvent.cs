using Bike4Me.Application.Abstractions.Messaging.Interfaces;

namespace Bike4Me.Application.Rentals.Events;

public sealed class RentalReturnedEvent(Guid rentalId, Guid bikeId) : Event
{
    public Guid RentalId => rentalId;
    public Guid BikeId => bikeId;
}