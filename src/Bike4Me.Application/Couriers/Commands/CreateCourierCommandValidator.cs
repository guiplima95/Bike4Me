using FluentValidation;
using System.Text.RegularExpressions;

namespace Bike4Me.Application.Couriers.Commands;

public class CreateCourierCommandValidator : AbstractValidator<CreateCourierCommand>
{
    private static readonly Regex CnpjRegex = new(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", RegexOptions.Compiled);
    private static readonly Regex CnhTypeRegex = new(@"^[A-Z]$", RegexOptions.Compiled);

    public CreateCourierCommandValidator()
    {
        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ is required.")
            .Must(IsValidCnpj)
            .WithMessage("CNPJ is invalid.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ is required.")
            .Matches(CnpjRegex).WithMessage("CNPJ format is invalid.");

        RuleFor(x => x.DateBirthday)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.CnhNumber)
            .NotEmpty().WithMessage("CNH number is required.");

        RuleFor(x => x.CnhType)
            .NotEmpty().WithMessage("CNH type is required.")
            .Matches(CnhTypeRegex).WithMessage("CNH type must be a single uppercase letter.");

        RuleFor(x => x.ImagemCnh)
            .NotEmpty().WithMessage("CNH image is required.")
            .Must(BeValidBase64).WithMessage("CNH image must be a valid base64 string.");
    }

    private bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            return false;
        }

        string cleaned = Regex.Replace(cnpj, @"[^\d]", "");

        if (cleaned.Length != 14)
        {
            return false;
        }

        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cleaned.Substring(0, 12);
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];
        }

        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCnpj += digit1;
        sum = 0;

        for (int i = 0; i < 13; i++)
        {
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
        }

        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        string digits = cleaned.Substring(12, 2);

        return digits == $"{digit1}{digit2}";
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