using FluentValidation;

namespace Bike4Me.Application.Rentals.Commands;

public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
{
    private static readonly int[] AllowedPlanDays = [7, 15, 30, 45, 50];

    public CreateRentalCommandValidator()
    {
        RuleFor(x => x.MotorcycleId)
            .NotEmpty()
            .WithMessage("BikeId is required.");

        RuleFor(x => x.CourierId)
            .NotEmpty()
            .WithMessage("CourierId is required.");

        RuleFor(x => x.RentalPlanDays)
            .Must(days =>
            {
                return AllowedPlanDays.Contains(days);
            })
            .WithMessage("Rental plan days must be one of the allowed values: 7, 15, 30, 45, 50.");

        RuleFor(x => x.RentalStartDate)
            .Must(date =>
            {
                return date.Date == DateTime.UtcNow.Date.AddDays(1);
            })
            .WithMessage("Rental start date must be the day after creation date.");

        RuleFor(x => x)
            .Must(x =>
            {
                return x.RentalEndDate >= x.RentalStartDate && x.ExpectedReturnDate >= x.RentalStartDate;
            })
            .WithMessage("Rental end date and expected return date must be greater than or equal to rental start date.");
    }
}