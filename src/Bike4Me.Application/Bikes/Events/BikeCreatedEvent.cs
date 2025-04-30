using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Domain.Bikes;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikeCreatedEvent(BikeReport bike) : Event
{
    public BikeReport Bike => bike;
}