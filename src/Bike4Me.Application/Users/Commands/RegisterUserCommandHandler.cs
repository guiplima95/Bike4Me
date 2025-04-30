using Bike4Me.Application.Abstractions.Security;
using Bike4Me.Domain.Users;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyExists);
        }

        Result<Email> emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(EmailErrors.InvalidFormat);
        }

        User user = User.Create(
           emailResult.Value,
           new Name(request.Name),
           UserRole.Client);

        var hashedPassword = passwordHasher.Hash(user, request.Password);

        user.SetPasswordHash(hashedPassword);

        await userRepository.AddAsync(user);

        return Result.Success(user.Id);
    }
}