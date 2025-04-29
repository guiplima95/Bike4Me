using SharedKernel;
using System.Text.RegularExpressions;

namespace Bike4Me.Domain.Couriers;

public sealed record Cnh
{
    private Cnh(string number, string category)
    {
        Number = number;
        Category = category;
    }

    public string Number { get; }
    public string Category { get; }

    public static Result<Cnh> Create(string? number, string? category)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return Result.Failure<Cnh>(CnhErrors.EmptyNumber);
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            return Result.Failure<Cnh>(CnhErrors.EmptyCategory);
        }

        if (!Regex.IsMatch(number!, @"^\d{11}$"))
        {
            return Result.Failure<Cnh>(CnhErrors.InvalidNumberFormat);
        }

        var validCategories = new[] { "A", "B", "A+B" };
        if (!validCategories.Contains(category))
        {
            return Result.Failure<Cnh>(CnhErrors.InvalidCategory);
        }

        return Result.Success(new Cnh(number, category));
    }
}