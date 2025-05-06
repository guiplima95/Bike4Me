using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class UpdateBikePlateCommandValidator : AbstractValidator<UpdateBikePlateCommand>
{
    private static readonly Regex PlateRegex = new(@"^([A-Z]{3}-\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", RegexOptions.Compiled);

    public UpdateBikePlateCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithErrorCode("UpdateMotorcyclePlate.MissingBikeId");

        RuleFor(c => c.LicensePlate)
            .NotEmpty()
            .WithMessage("LicensePlate is required.")
            .Matches(PlateRegex).WithMessage("LicensePlate must follow the old format (ABC-1234) or Mercosul format (ABC1D23).").Matches(PlateRegex).WithMessage("LicensePlate must follow the old format (ABC-1234) or Mercosul format (ABC1D23).")
            .MaximumLength(8)
            .WithErrorCode("UpdateMotorcyclePlate.InvalidPlateLength");
    }
}