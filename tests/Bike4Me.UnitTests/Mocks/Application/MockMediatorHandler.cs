using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Events;
using Moq;

namespace Bike4Me.UnitTests.Mocks.Application;

public sealed class MockMediatorHandler : Mock<IMediatorHandler>
{
    public MockMediatorHandler() : base(MockBehavior.Strict)
    {
    }

    public MockMediatorHandler MockPublishBikeCreatedEvent()
    {
        var setup = Setup(m => m.PublishEvent(It.IsAny<BikeCreatedEvent>()))
                       .Returns(Task.CompletedTask);

        return this;
    }

    public MockMediatorHandler VerifyPublishBikeCreatedEvent(Times times)
    {
        Verify(m => m.PublishEvent(It.IsAny<BikeCreatedEvent>()), times);
        return this;
    }
}