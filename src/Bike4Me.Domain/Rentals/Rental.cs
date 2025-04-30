using Bike4Me.Domain.Bikes;
using SharedKernel;

namespace Bike4Me.Domain.Rentals;

public class Rental : Entity
{
    // EF Core constructor
    protected Rental()
    {
    }

    private Rental(
        Guid id,
        Guid bikeId,
        Guid courierId,
        RentalPlan rentalPlan,
        DateTime rentalStartDate,
        DateTime expectedReturnDate)
    {
        Id = id;
        BikeId = bikeId;
        CourierId = courierId;
        RentalPlan = rentalPlan;
        RentalStartDate = rentalStartDate;
        ExpectedReturnDate = expectedReturnDate;
        RentalEndDate = rentalStartDate.AddDays(rentalPlan.Days);
        Status = RentalStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid BikeId { get; private set; }
    public Guid CourierId { get; private set; }
    public RentalPlan RentalPlan { get; private set; } = null!;
    public DateTime RentalStartDate { get; private set; }
    public DateTime RentalEndDate { get; private set; }
    public DateTime ExpectedReturnDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }
    public RentalStatus Status { get; private set; }
    public decimal? TotalPrice { get; private set; }

    public static Result<Rental> Create(
        Guid motorcycleId,
        Guid courierId,
        RentalPlan rentalPlan,
        DateTime rentalStartDate)
    {
        if (rentalStartDate.Date != DateTime.UtcNow.Date.AddDays(1))
        {
            return Result.Failure<Rental>(RentalErrors.InvalidRentalStartDate);
        }

        var expectedReturnDate = rentalStartDate.AddDays(rentalPlan.Days);

        var rental = new Rental(
            Guid.NewGuid(),
            motorcycleId,
            courierId,
            rentalPlan,
            rentalStartDate,
            expectedReturnDate);

        return Result.Success(rental);
    }

    public Result UpdateExpectedReturnDate(DateTime newExpectedReturnDate)
    {
        if (newExpectedReturnDate < RentalStartDate)
        {
            return Result.Failure(RentalErrors.ExpectedReturnDateBeforeStart);
        }

        ExpectedReturnDate = newExpectedReturnDate;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result ReturnBike(DateTime actualReturnDate)
    {
        if (actualReturnDate < RentalStartDate)
        {
            return Result.Failure(RentalErrors.ActualReturnDateBeforeStart);
        }

        ActualReturnDate = actualReturnDate;
        Status = RentalStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
        TotalPrice = CalculateTotalPrice();

        return Result.Success();
    }

    public Result UpdateRentalEndDate(DateTime newRentalEndDate)
    {
        if (newRentalEndDate < RentalStartDate)
        {
            return Result.Failure(RentalErrors.ExpectedReturnDateBeforeStart);
        }

        RentalEndDate = newRentalEndDate;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    private decimal CalculateTotalPrice()
    {
        Ensure.NotNull(ActualReturnDate);

        var daysRented = (ActualReturnDate.Value.Date - RentalStartDate.Date).Days + 1;
        var dailyRate = RentalPlan.DailyRate;
        var basePrice = daysRented * dailyRate;

        if (ActualReturnDate.Value.Date < ExpectedReturnDate.Date)
        {
            var unusedDays = (ExpectedReturnDate.Date - ActualReturnDate.Value.Date).Days;
            decimal penaltyRate = RentalPlan.PenaltyPercentage / 100m;
            var penalty = unusedDays * dailyRate * penaltyRate;
            return basePrice + penalty;
        }

        if (ActualReturnDate.Value.Date > ExpectedReturnDate.Date)
        {
            var extraDays = (ActualReturnDate.Value.Date - ExpectedReturnDate.Date).Days;
            var extraFee = extraDays * RentalPlan.AdditionalDailyFee;
            return basePrice + extraFee;
        }

        return basePrice;
    }
}