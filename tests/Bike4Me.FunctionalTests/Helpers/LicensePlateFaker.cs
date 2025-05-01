using Bike4Me.Domain.Bikes;
using Bogus;

namespace Bike4Me.FunctionalTests.Helpers;

public static class LicensePlateFaker
{
    private static readonly Faker Faker = new();

    public static LicensePlate GenerateOldFormat()
    {
        var letters = Faker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var numbers = Faker.Random.Number(1000, 9999).ToString();
        var plateStr = $"{letters}-{numbers}";

        return new LicensePlate(plateStr);
    }

    public static LicensePlate GenerateMercosulFormat()
    {
        var letters1 = Faker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var digit = Faker.Random.Number(0, 9);
        var letter2 = Faker.Random.Char('A', 'Z');
        var numbers = Faker.Random.Number(10, 99).ToString();
        var plateStr = $"{letters1}{digit}{letter2}{numbers}";

        return new LicensePlate(plateStr);
    }

    public static LicensePlate GenerateRandom()
    {
        return Faker.Random.Bool() ? GenerateOldFormat() : GenerateMercosulFormat();
    }
}