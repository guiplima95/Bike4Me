using Bike4Me.Domain.Users;
using FluentAssertions;

namespace Bike4Me.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void Create_ShouldInitializeFields()
    {
        var id = Guid.NewGuid();
        var mail = Email.Create("jane@doe.com");
        var name = new Bike4Me.Domain.Users.Name("Jane Doe");

        var user = User.Create(id, mail.Value, name, UserRole.Admin);

        user.Id.Should().Be(id);
        user.Email.Should().Be(mail.Value);
        user.Name.Should().Be(name);
        user.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public void SetRole_ShouldUpdateRoleAndTimestamp()
    {
        var user = User.Create(Guid.NewGuid(), Email.Create("x@x.com").Value, new Bike4Me.Domain.Users.Name("X"), null);
        var previous = user.UpdatedAt;

        user.SetRole(UserRole.Admin);

        user.Role.Should().Be(UserRole.Admin);
        user.UpdatedAt.Should().BeAfter(previous);
    }

    [Fact]
    public void SetPasswordHash_ShouldStoreHash()
    {
        var user = User.Create(Guid.NewGuid(), Email.Create("x@x.com").Value, new Bike4Me.Domain.Users.Name("X"), null);
        var hash = "hashed_pwd";

        user.SetPasswordHash(hash);

        user.GetPasswordHash().Should().Be(hash);
    }
}