using Bike4Me.Domain.Bikes;
using FluentAssertions;

namespace Bike4Me.UnitTests.Domain;

public sealed class BikeTests
{
    [Fact]
    public void Create_ShouldInitializeAllFields()
    {
        var id = Guid.NewGuid();
        var plate = new LicensePlate("ABC1D23");
        var modelId = Guid.NewGuid();
        var color = "Black";

        var before = DateTime.UtcNow;
        var bike = Bike.Create(id, plate, modelId, color);
        var after = DateTime.UtcNow;

        bike.Id.Should().Be(id);
        bike.Plate.Should().Be(plate);
        bike.ModelId.Should().Be(modelId);
        bike.Color.Should().Be(color);
        bike.Status.Should().Be(BikeStatus.Available);
        bike.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        bike.UpdatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void UpdatePlate_ShouldChangePlateAndTimestamp()
    {
        var bike = Bike.Create(Guid.NewGuid(), new LicensePlate("ABC1D23"), Guid.NewGuid(), "Blue");
        var previous = bike.UpdatedAt;

        var newPlate = new LicensePlate("DEF2G34");
        bike.UpdatePlate(newPlate);

        bike.Plate.Should().Be(newPlate);
        bike.UpdatedAt.Should().BeAfter(previous);
    }

    [Fact]
    public void MarkAsRentedAndAvailable_ShouldToggleStatus()
    {
        var bike = Bike.Create(Guid.NewGuid(), new LicensePlate("ABC1D23"), Guid.NewGuid(), "Blue");

        bike.MarkAsRented();
        bike.Status.Should().Be(BikeStatus.Rented);

        bike.MarkAsAvailable();
        bike.Status.Should().Be(BikeStatus.Available);
    }

    [Fact]
    public void Build_ShouldReturnBikeReportWithExpectedValues()
    {
        var id = Guid.NewGuid();

        var report = Bike.Build(id, "ABC1D23", "CB300", 2024);

        report.Id.Should().Be(id.ToString());
        report.LicensePlate.Should().Be("ABC1D23");
        report.ModelName.Should().Be("CB300");
        report.Year.Should().Be(2024);
    }
}
