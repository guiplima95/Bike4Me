using Bike4Me.Application.Bikes.Commands;
using Bike4Me.UnitTests.Builders;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Bike4Me.UnitTests.Application;

public sealed class CreateBikeCommandValidatorTests
{
    private readonly CreateBikeCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new CreateBikeCommandBuilder().Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345678")]
    public void Validate_WhenPlateIsInvalid_ShouldReturnError(string plate)
    {
        var command = new CreateBikeCommandBuilder()
                     .WithPlate(plate)
                     .Build();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Plate);
    }

    [Fact]
    public void Validate_WhenColorIsEmpty_ShouldReturnError()
    {
        var command = new CreateBikeCommandBuilder()
                     .WithColor(string.Empty)
                     .Build();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Color);
    }

    [Theory]
    [InlineData(1899)]
    [InlineData(3000)]
    public void Validate_WhenYearOutOfRange_ShouldReturnError(int year)
    {
        var command = new CreateBikeCommandBuilder()
                     .WithYear(year)
                     .Build();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.ModelYear);
    }

    [Fact]
    public void Validate_WhenEngineCapacityTooLong_ShouldReturnError()
    {
        var command = new CreateBikeCommandBuilder()
                     .WithEngineCapacity(new string('X', 21))  // > 20 chars
                     .Build();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.ModelEngineCapacity);
    }
}