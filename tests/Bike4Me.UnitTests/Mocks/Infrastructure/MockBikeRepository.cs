using Bike4Me.Domain.Bikes;
using Moq;

namespace Bike4Me.UnitTests.Mocks.Infrastructure;

public sealed class MockBikeRepository : Mock<IBikeRepository>
{
    public MockBikeRepository() : base(MockBehavior.Strict)
    {
    }

    public MockBikeRepository MockAnyExistsAsync(string plate, bool exists)
    {
        Setup(r => r.AnyExistsAsync(plate)).ReturnsAsync(exists);
        return this;
    }

    public MockBikeRepository MockAddAsyncWithCallback(Action<Bike> callback)
    {
        Setup(r => r.AddAsync(It.IsAny<Bike>()))
            .Returns(Task.CompletedTask)
            .Callback(callback);
        return this;
    }

    public MockBikeRepository VerifyAddAsync(Times times)
    {
        Verify(r => r.AddAsync(It.IsAny<Bike>()), times);
        return this;
    }
}