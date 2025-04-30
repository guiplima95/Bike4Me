using Bike4Me.Domain.Users;
using SharedKernel;
using System.Text.RegularExpressions;

namespace Bike4Me.Domain.Couriers;

public class Courier : Entity
{
    // EF Core constructor
    protected Courier()
    {
    }

    private Courier(Guid id, Email email, Name name, Cnh cnh, Cnpj cnpj)
    {
        Id = id;
        Email = email;
        Name = name;
        Cnh = cnh;
        Cnpj = cnpj;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Email Email { get; private set; } = null!;
    public Name Name { get; private set; } = null!;
    public Cnh Cnh { get; private set; } = null!;
    public Cnpj Cnpj { get; private set; } = null!;

    public static Courier Create(Guid id, Email email, Name name, Cnh cnh, Cnpj cnpj)
    {
        return new Courier(id, email, name, cnh, cnpj);
    }
}