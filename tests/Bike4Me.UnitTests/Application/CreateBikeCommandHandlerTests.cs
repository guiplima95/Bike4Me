using Bike4Me.Application.Bikes.Commands;
using Bike4Me.Application.Bikes.Events;
using Bike4Me.Domain.Bikes;
using Bike4Me.UnitTests.Builders;
using Bike4Me.UnitTests.Mocks.Application;
using Bike4Me.UnitTests.Mocks.Infrastructure;
using FluentAssertions;
using Moq;

namespace Bike4Me.UnitTests.Application;

public sealed class CreateBikeCommandHandlerTests
{
    private readonly MockBikeModelRepository _bikeModelRepository = new();
    private readonly MockBikeRepository _bikeRepository = new();
    private readonly MockMediatorHandler _mediator = new();
    private readonly CreateBikeCommandHandler _handler;

    public CreateBikeCommandHandlerTests()
    {
        _handler = new CreateBikeCommandHandler(
                   _bikeModelRepository.Object,
                   _bikeRepository.Object,
                   _mediator.Object);
    }

    [Fact]
    public async Task Handle_CreateNewBike_ShouldPersistAndPublishEvent()
    {
        // Arrange
        var command = new CreateBikeCommandBuilder().Build();

        BikeModel? capturedModel = null;
        Bike? capturedBike = null;

        _bikeModelRepository
            .MockGetModelAsync(
                new Name(command.ModelName),
                new Manufacturer(command.ModelManufacturer),
                new Year(command.ModelYear),
                command.ModelEngineCapacity,
                output: null)
            .MockAddAsyncWithCallback(m => capturedModel = m);

        _bikeRepository
            .MockAnyExistsAsync(command.Plate, exists: false)
            .MockAddAsyncWithCallback(b => capturedBike = b);

        _mediator
            .MockPublishBikeCreatedEvent();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _bikeModelRepository.VerifyAddAsync(Times.Once());
        _bikeRepository.VerifyAddAsync(Times.Once());
        _mediator.VerifyPublishBikeCreatedEvent(Times.Once());
    }
}