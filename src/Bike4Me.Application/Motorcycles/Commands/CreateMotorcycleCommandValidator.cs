using FluentValidation;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
{
    public CreateMotorcycleCommandValidator()
    {
        RuleFor(c => c.Plate)
            .NotEmpty()
            .WithErrorCode("CreateMotorcycle.MissingPlate")
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
