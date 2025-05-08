using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using Bike4Me.UnitTests.Mocks.Infrastructure;
using FluentAssertions;
using Moq;

namespace Bike4Me.UnitTests.Application;

public sealed class BikeCreatedEventHandlerTests
{
    private readonly MockBikeReportRepository _bikeReportRepository = new();
    private readonly BikeCreatedEventHandler _handler;

    public BikeCreatedEventHandlerTests()
    {
        _handler = new BikeCreatedEventHandler(_bikeReportRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpsertBikeReport()
    {
        // Arrange
        var bikeReport = Bike.Build(Guid.NewGuid(), "ABC1D23", "CB300", 2024);
        var @event = new BikeCreatedEvent(bikeReport);
        BikeReport? actual = null;

        _bikeReportRepository
            .MockUpsertAsyncWithCallback(r => actual = r);

        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(bikeReport);
        _bikeReportRepository.VerifyUpsertAsync(bikeReport, Times.Once());
    }
}