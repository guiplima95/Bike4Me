using FluentValidation;

namespace Bike4Me.Application.Motorcycles.Commands;

public sealed class UpdateMotorcyclePlateCommandValidator : AbstractValidator<UpdateMotorcyclePlateCommand>
{
    public UpdateMotorcyclePlateCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithErrorCode("UpdateMotorcyclePlate.MissingMotorcycleId");

        RuleFor(c => c.Plate)
            .NotEmpty()
            .WithErrorCode("UpdateMotorcyclePlate.MissingPlate")
            .MaximumLength(8)
            .WithErrorCode("UpdateMotorcyclePlate.InvalidPlateLength");
    }
}