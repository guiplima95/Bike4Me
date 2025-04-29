using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
{
    private static readonly Regex PlateRegex = new(@"^([A-Z]{3}-\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", RegexOptions.Compiled);

    public CreateMotorcycleCommandValidator()
    {
        RuleFor(c => c.Plate)
            .NotEmpty().WithMessage("Plate is required.")
            .Matches(PlateRegex).WithMessage("Plate must follow the old format (ABC-1234) or Mercosul format (ABC1D23).")
            .MaximumLength(8)
            .WithErrorCode("CreateMotorcycle.InvalidPlateLength");

        RuleFor(c => c.Color)
            .NotEmpty()
            .WithErrorCode("CreateMotorcycle.MissingColor")
            .MaximumLength(30)
            .WithErrorCode("CreateMotorcycle.InvalidColorLength");

        RuleFor(c => c.ModelName)
            .NotEmpty()
            .WithErrorCode("CreateMotorcycle.MissingModelName")
            .MaximumLength(50)
            .WithErrorCode("CreateMotorcycle.InvalidModelNameLength");

        RuleFor(c => c.ModelManufacturer)
            .NotEmpty()
            .WithErrorCode("CreateMotorcycle.MissingModelManufacturer")
            .MaximumLength(50)
            .WithErrorCode("CreateMotorcycle.InvalidModelManufacturerLength");

        RuleFor(c => c.ModelYear)
            .NotNull()
            .WithErrorCode("CreateMotorcycle.MissingModelYear")
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithErrorCode("CreateMotorcycle.InvalidModelYear");

        RuleFor(c => c.ModelEngineCapacity)
            .NotEmpty()
            .WithErrorCode("CreateMotorcycle.MissingModelEngineCapacity")
            .MaximumLength(20)
            .WithErrorCode("CreateMotorcycle.InvalidModelEngineCapacityLength");
    }
}