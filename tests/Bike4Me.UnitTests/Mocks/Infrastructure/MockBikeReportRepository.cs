using Bike4Me.Domain.Bikes;
using Moq;

namespace Bike4Me.UnitTests.Mocks.Infrastructure;

public sealed class MockBikeReportRepository : Mock<IBikeReportRepository>
{
    public MockBikeReportRepository() : base(MockBehavior.Strict)
    {
    }

    public MockBikeReportRepository MockUpsertAsyncWithCallback(Action<BikeReport>? callback = null)
    {
        var setup = Setup(r => r.UpsertAsync(It.IsAny<BikeReport>()))
                       .Returns(Task.CompletedTask);

        if (callback is not null)
            setup.Callback(callback);

        return this;
    }

    public MockBikeReportRepository VerifyUpsertAsync(BikeReport expected, Times times)
    {
        Verify(r => r.UpsertAsync(It.Is<BikeReport>(b =>
            b.Id == expected.Id &&
            b.LicensePlate == expected.LicensePlate &&
            b.ModelName == expected.ModelName &&
            b.Year == expected.Year)), times);
        return this;
    }
}