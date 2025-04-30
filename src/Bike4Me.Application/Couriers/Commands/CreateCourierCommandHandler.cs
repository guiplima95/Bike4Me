using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Domain.Couriers;
using Bike4Me.Domain.Users;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Couriers.Commands;

public class CreateCourierCommandHandler(
    ICourierRepository courierRepository,
    IMediatorHandler mediator) : IRequestHandler<CreateCourierCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        Name name = new(request.Name);
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        var cnhResult = Cnh.Create(request.CnhNumber, request.CnhType);
        if (cnhResult.IsFailure)
        {
            return Result.Failure<Guid>(cnhResult.Error);
        }

        Cnpj cnpj = new(request.Cnpj);

        var courierId = Guid.NewGuid();

        if (await courierRepository.ExistsByCnpjAsync(request.Cnpj))
        {
            return Result.Failure<Guid>(CourierErrors.DuplicateCnpj);
        }

        await mediator.PublishCommand(new ImportCourierCnhCommand(courierId, request.ImagemCnh));

        Courier courier = Courier.Create(courierId, emailResult.Value, name, cnhResult.Value, cnpj);

        await courierRepository.AddAsync(courier);

        return Result.Success(courier.Id);
    }
}