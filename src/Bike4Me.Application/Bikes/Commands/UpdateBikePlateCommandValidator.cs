using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class UpdateBikePlateCommandValidator : AbstractValidator<UpdateBikePlateCommand>
{
    private static readonly Regex PlateRegex = new(@"^[A-Z]{3}-\d{4}$", RegexOptions.Compiled);

    public UpdateBikePlateCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithErrorCode("UpdateMotorcyclePlate.MissingMotorcycleId");

        RuleFor(c => c.Plate)
            .NotEmpty()
            .WithMessage("LicensePlate is required.")
            .Matches(PlateRegex).WithMessage("LicensePlate must be in the format ABC-1234.")
            .MaximumLength(8)
            .WithErrorCode("UpdateMotorcyclePlate.InvalidPlateLength");
    }
}