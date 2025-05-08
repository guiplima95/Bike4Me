using Bike4Me.Domain.Couriers;
using Bike4Me.Domain.Users;
using FluentAssertions;

namespace Bike4Me.UnitTests.Domain;

public sealed class CourierTests
{
    [Fact]
    public void Create_ShouldPopulateFields()
    {
        var id = Guid.NewGuid();
        var mail = Email.Create("john@doe.com");
        var name = new Name("John Doe");
        var cnh = Cnh.Create("12345678900", "A");
        var cnpj = new Cnpj("12.345.678/0001-00");

        var before = DateTime.UtcNow;
        var courier = Courier.Create(id, mail.Value, name, cnh.Value, cnpj);
        var after = DateTime.UtcNow;

        courier.Id.Should().Be(id);
        courier.Email.Should().Be(mail.Value);
        courier.Name.Should().Be(name);
        courier.Cnh.Should().Be(cnh.Value);
        courier.Cnpj.Should().Be(cnpj);
        courier.CreatedAt.Should()
            .BeOnOrAfter(before)
            .And
            .BeOnOrBefore(after);
    }
}