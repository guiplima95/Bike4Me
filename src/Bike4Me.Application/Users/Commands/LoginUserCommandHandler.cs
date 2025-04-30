using Bike4Me.Application.Abstractions.Security;
using Bike4Me.Domain.Users;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenGenerator) : IRequestHandler<LoginUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        if (!passwordHasher.Verify(user, user.GetPasswordHash(), request.Password))
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        string token = tokenGenerator.GenerateToken(user);

        return Result.Success(token);
    }
}