using FluentValidation;

namespace Bike4Me.Application.Couriers.Commands;

public class ImportCourierCnhCommandValidator : AbstractValidator<ImportCourierCnhCommand>
{
    public ImportCourierCnhCommandValidator()
    {
        RuleFor(x => x.ImagemCnh)
            .NotEmpty().WithMessage("CNH image is required.")
            .Must(BeValidBase64).WithMessage("CNH image must be a valid base64 string.");
    }

    private bool BeValidBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return false;
        }

        Span<byte> buffer = new(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}