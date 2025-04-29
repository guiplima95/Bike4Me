using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class UpdateMotorcyclePlateCommandValidator : AbstractValidator<UpdateMotorcyclePlateCommand>
{
    private static readonly Regex PlateRegex = new(@"^[A-Z]{3}-\d{4}$", RegexOptions.Compiled);

    public UpdateMotorcyclePlateCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithErrorCode("UpdateMotorcyclePlate.MissingMotorcycleId");

        RuleFor(c => c.Plate)
            .NotEmpty()
            .WithMessage("Plate is required.")
            .Matches(PlateRegex).WithMessage("Plate must be in the format ABC-1234.")
            .MaximumLength(8)
            .WithErrorCode("UpdateMotorcyclePlate.InvalidPlateLength");
    }
}