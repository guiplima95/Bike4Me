using SharedKernel;

namespace Bike4Me.Domain.Rentals;

public class RentalPlan(int days, decimal dailyRate, decimal penaltyPercentage, decimal additionalDailyFee)
{
    public int Days => days;
    public decimal DailyRate => dailyRate;
    public decimal PenaltyPercentage => penaltyPercentage;
    public decimal AdditionalDailyFee => additionalDailyFee;
}

public static class RentalErrors
{
    public static readonly Error InvalidRentalStartDate = Error.Problem(
        "Rental.InvalidRentalStartDate", "Rental start date must be the day after creation date.");

    public static readonly Error ExpectedReturnDateBeforeStart = Error.Problem(
        "Rental.ExpectedReturnDateBeforeStart", "Expected return date cannot be before rental start date.");

    public static readonly Error ActualReturnDateBeforeStart = Error.Problem(
        "Rental.ActualReturnDateBeforeStart", "Actual return date cannot be before rental start date.");

    public static readonly Error CannotCalculatePriceWithoutReturnDate = Error.Problem(
        "Rental.CannotCalculatePriceWithoutReturnDate", "Cannot calculate total price without actual return date.");
}