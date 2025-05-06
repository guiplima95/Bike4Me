using Bike4Me.Domain.Bikes;
using Moq;

namespace Bike4Me.UnitTests.Mocks.Infrastructure;

public sealed class MockBikeModelRepository : Mock<IBikeModelRepository>
{
    public MockBikeModelRepository() : base(MockBehavior.Strict)
    {
    }

    public MockBikeModelRepository MockGetModelAsync(
        Name name,
        Manufacturer manufacturer,
        Year year,
        string capacity,
        BikeModel? output)
    {
        Setup(r => r.GetModelAsync(name, manufacturer, year, capacity))
            .ReturnsAsync(output);
        return this;
    }

    public MockBikeModelRepository MockAddAsyncWithCallback(Action<BikeModel> callback)
    {
        Setup(r => r.AddAsync(It.IsAny<BikeModel>()))
            .Returns(Task.CompletedTask)
            .Callback(callback);

        return this;
    }

    public MockBikeModelRepository VerifyAddAsync(Times times)
    {
        Verify(r => r.AddAsync(It.IsAny<BikeModel>()), times);

        return this;
    }
}