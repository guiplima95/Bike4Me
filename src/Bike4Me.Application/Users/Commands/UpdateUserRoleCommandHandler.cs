using Bike4Me.Domain.Users;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public class UpdateUserRoleCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserRoleCommand, Result>
{
    public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return Result.Failure<Unit>(UserErrors.UserNotFound);
        }

        user.SetRole(request.Role);

        await userRepository.UpdateAsync(user);

        return Result.Success();
    }
}