using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Bikes.Commands;

public sealed class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
{
    private static readonly Regex PlateRegex = new(@"^([A-Z]{3}-\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", RegexOptions.Compiled);

    public CreateBikeCommandValidator()
    {
        RuleFor(c => c.Plate)
            .NotEmpty().WithMessage("LicensePlate is required.")
            .Matches(PlateRegex).WithMessage("LicensePlate must follow the old format (ABC-1234) or Mercosul format (ABC1D23).")
            .MaximumLength(8)
            .WithErrorCode("CreateBike.InvalidPlateLength");

        RuleFor(c => c.Color)
            .NotEmpty()
            .WithErrorCode("CreateBike.MissingColor")
            .MaximumLength(30)
            .WithErrorCode("CreateBikeCreateBike.InvalidColorLength");

        RuleFor(c => c.ModelName)
            .NotEmpty()
            .WithErrorCode("CreateBike.MissingModelName")
            .MaximumLength(50)
            .WithErrorCode("CreateBike.InvalidModelNameLength");

        RuleFor(c => c.ModelManufacturer)
            .NotEmpty()
            .WithErrorCode("CreateBike.MissingModelManufacturer")
            .MaximumLength(50)
            .WithErrorCode("CreateBike.InvalidModelManufacturerLength");

        RuleFor(c => c.ModelYear)
            .NotNull()
            .WithErrorCode("CreateBike.MissingModelYear")
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithErrorCode("CreateBike.InvalidModelYear");

        RuleFor(c => c.ModelEngineCapacity)
            .NotEmpty()
            .WithErrorCode("CreateBike.MissingModelEngineCapacity")
            .MaximumLength(20)
            .WithErrorCode("CreateBike.InvalidModelEngineCapacityLength");
    }
}