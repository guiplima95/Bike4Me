using Bike4Me.Domain.Rentals;
using Bike4Me.IntegrationTests.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.IntegrationTests.Queries;

public class RentalsQueryTests(IntegrationTestWebAppFactory<Program> factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task FindAByIdAsync_WithExistingRental_ShouldReturnRental()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var bikeId = Guid.NewGuid();
        var courierId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date.AddDays(1);
        var endDate = startDate.AddDays(7);
        var rentalPlanDays = 7;
        var rentalPlanDailyRate = 25.0m;
        var rentalPlanPenaltyPercentage = 20.0m;
        var rentalPlanAdditionalDailyFee = 35.0m;

        await Bike4MeContext.Database.ExecuteSqlRawAsync(@"
            INSERT INTO bike4me.rentals (
                id, bike_id, courier_id, rental_start_date, rental_end_date,
                expected_return_date, status, created_at, updated_at,
                rental_plan_days, rental_plan_daily_rate, rental_plan_penalty_percentage, rental_plan_additional_daily_fee
            ) VALUES (
                {0}, {1}, {2}, {3}, {4},
                {5}, {6}, {7}, {8},
                {9}, {10}, {11}, {12} )",

            rentalId, bikeId, courierId, startDate, endDate,
            endDate, (int)RentalStatus.Active, DateTime.UtcNow, DateTime.UtcNow,
            rentalPlanDays, rentalPlanDailyRate, rentalPlanPenaltyPercentage, rentalPlanAdditionalDailyFee);

        // Act
        var result = await RentalsQuery.FindAByIdAsync(rentalId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(rentalId);
        result.Value.BikeId.Should().Be(bikeId);
        result.Value.CourierId.Should().Be(courierId);
        result.Value.RentalStartDate.Should().Be(startDate);
        result.Value.RentalEndDate.Should().Be(endDate);
        result.Value.ExpectedReturnDate.Should().Be(endDate);
        result.Value.Status.Should().Be(RentalStatus.Active);
    }

    [Fact]
    public async Task FindAByIdAsync_WithNonExistingRental_ShouldReturnFailure()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await RentalsQuery.FindAByIdAsync(nonExistingId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(RentalErrors.NotFound);
    }
}