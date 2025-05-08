using SharedKernel;

namespace Bike4Me.Domain.Users;

public class User : Entity
{
    // EF Core constructor
    protected User()
    {
    }

    private User(Guid id, Email email, Name name, UserRole? role)
    {
        Id = id;
        Email = email;
        Name = name;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Email Email { get; private set; } = null!;

    public Name Name { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public UserRole? Role { get; private set; }

    public void SetRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public string GetPasswordHash()
    {
        return PasswordHash;
    }

    public void SetPasswordHash(string hashedPassword)
    {
        Ensure.NotNullOrEmpty(hashedPassword, nameof(hashedPassword));

        PasswordHash = hashedPassword;
    }

    public static User Create(Guid id, Email email, Name name, UserRole? role)
    {
        var user = new User(id, email, name, role);

        return user;
    }
}