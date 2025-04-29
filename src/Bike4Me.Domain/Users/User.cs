using SharedKernel;

namespace Bike4Me.Domain.Users;

public class User : Entity
{
    // EF Core constructor
    protected User()
    {
    }

    private User(Guid id, Email email, Name name)
    {
        Id = id;
        Email = email;
        Name = name;
    }

    public Email Email { get; private set; } = null!;

    public Name Name { get; private set; } = null!;

    public static User Create(Email email, Name name)
    {
        var user = new User(Guid.NewGuid(), email, name);

        return user;
    }
}