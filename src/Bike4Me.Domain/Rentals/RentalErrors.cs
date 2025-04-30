using SharedKernel;

namespace Bike4Me.Domain.Rentals;

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

    public static readonly Error CourierNotAuthorized = Error.Problem(
        "Rental.CourierNotAuthorized", "Courier must be licensed in category A to rent a bike.");

    public static readonly Error InvalidPlan = Error.Problem(
        "Rental.InvalidPlan", "Invalid rental plan days.");

    public static readonly Error NotFound = Error.Problem(
        "Rental.NotFound", "Rental not found.");
}