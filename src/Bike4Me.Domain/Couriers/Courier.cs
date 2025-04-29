using Bike4Me.Domain.Users;
using SharedKernel;

namespace Bike4Me.Domain.Couriers;

public class Courier : Entity
{
    // EF Core constructor
    protected Courier()
    {
    }

    private Courier(Guid id, Email email, Name name, Cnh cnh)
    {
        Id = id;
        Email = email;
        Name = name;
        Cnh = cnh;
    }

    public Email Email { get; private set; } = null!;
    public Name Name { get; private set; } = null!;
    public Cnh Cnh { get; private set; } = null!;

    public static Courier Create(Email email, Name name, Cnh cnh)
    {
        return new Courier(Guid.NewGuid(), email, name, cnh);
    }
}