namespace Bike4Me.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<bool> IsEmailUniqueAsync(Email email);

    Task AddAsync(User user);

    Task UpdateAsync(User user);
}