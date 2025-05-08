using Bike4Me.Domain.Rentals;
using FluentAssertions;

namespace Bike4Me.UnitTests.Domain;

public sealed class RentalTests
{
    private static RentalPlan Plan(int days) => days switch
    {
        // days rate penalty fee
        7 => new RentalPlan(7, 30m, 0m, 20m),
        15 => new RentalPlan(15, 28m, 0m, 40m),
        30 => new RentalPlan(30, 22m, 0m, 0m),
        45 => new RentalPlan(45, 20m, 0m, 0m),
        50 => new RentalPlan(50, 18m, 0m, 0m),
        _ => throw new ArgumentOutOfRangeException(nameof(days))
    };

    [Fact]
    public void Create_WithStartDateNotTomorrow_ShouldFail()
    {
        var today = DateTime.UtcNow;
        var res = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), Plan(7), today);

        res.IsSuccess.Should().BeFalse();
        res.Error.Should().Be(RentalErrors.InvalidRentalStartDate);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(50)]
    public void Create_WithValidStartDate_ShouldSucceed(int days)
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var plan = Plan(days);

        var res = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), plan, start);

        res.IsSuccess.Should().BeTrue();
        var rental = res.Value;

        rental.RentalPlan.Should().Be(plan);
        rental.RentalStartDate.Should().Be(start);
        rental.ExpectedReturnDate.Should().Be(start.AddDays(days));
        rental.Status.Should().Be(RentalStatus.Active);
    }

    [Fact]
    public void UpdateExpectedReturnDate_BeforeStart_ShouldFail()
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), Plan(7), start).Value;

        var res = rental.UpdateExpectedReturnDate(start.AddDays(-1));

        res.IsSuccess.Should().BeFalse();
        res.Error.Should().Be(RentalErrors.ExpectedReturnDateBeforeStart);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(15)]
    [InlineData(30)]
    public void ReturnBike_Early_ShouldChargeBasePrice(int days)
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var plan = Plan(days);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), plan, start).Value;

        var earlyReturn = start;                           // same calendar day
        rental.ReturnBike(earlyReturn).IsSuccess.Should().BeTrue();

        var expected = 1 * plan.DailyRate;                // inclusive counting → 1 day
        rental.TotalPrice.Should().Be(expected);
    }

    [Theory]
    [InlineData(7, 2, 20)]   // planDays, lateDays, extraFee
    [InlineData(15, 3, 40)]
    public void ReturnBike_Late_ShouldAddExtraFee(int days, int lateDays, decimal fee)
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var plan = Plan(days);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), plan, start).Value;

        var actualReturn = start.AddDays(days + lateDays);
        rental.ReturnBike(actualReturn).IsSuccess.Should().BeTrue();

        var rentedDays = days + lateDays + 1;              // inclusive
        var expected = rentedDays * plan.DailyRate + lateDays * fee;
        rental.TotalPrice.Should().Be(expected);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(50)]
    public void ReturnBike_Late_WithZeroFee_ShouldNotIncreasePrice(int days)
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var plan = Plan(days);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), plan, start).Value;

        var lateDays = 2;
        var actualReturn = start.AddDays(days + lateDays);
        rental.ReturnBike(actualReturn).IsSuccess.Should().BeTrue();

        var rentedDays = days + lateDays + 1;
        var expected = rentedDays * plan.DailyRate;
        rental.TotalPrice.Should().Be(expected);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(50)]
    public void ReturnBike_OnExpectedDate_ShouldChargeBasePrice(int days)
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var plan = Plan(days);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), plan, start).Value;

        var actualReturn = start.AddDays(days);
        rental.ReturnBike(actualReturn).IsSuccess.Should().BeTrue();

        var rentedDays = days + 1;                         // inclusive
        var expected = rentedDays * plan.DailyRate;
        rental.TotalPrice.Should().Be(expected);
    }

    [Fact]
    public void UpdateRentalEndDate_BeforeStart_ShouldFail()
    {
        var start = DateTime.UtcNow.Date.AddDays(1);
        var rental = Rental.Create(Guid.NewGuid(), Guid.NewGuid(), Plan(7), start).Value;

        var res = rental.UpdateRentalEndDate(start.AddDays(-1));

        res.IsSuccess.Should().BeFalse();
        res.Error.Should().Be(RentalErrors.ExpectedReturnDateBeforeStart);
    }
}