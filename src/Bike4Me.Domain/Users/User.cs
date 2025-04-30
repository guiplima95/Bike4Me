using SharedKernel;

namespace Bike4Me.Domain.Users;

public class User : Entity
{
    // EF Core constructor
    protected User()
    {
    }

    private User(Guid id, Email email, Name name, UserRole role)
    {
        Id = id;
        Email = email;
        Name = name;
        Role = role;
    }

    public Email Email { get; private set; } = null!;

    public Name Name { get; private set; } = null!;

    public UserRole Role { get; private set; }

    public static User Create(Email email, Name name, UserRole role)
    {
        var user = new User(Guid.NewGuid(), email, name, role);

        return user;
    }
}