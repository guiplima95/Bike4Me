using Bike4Me.Application.Abstractions.Messaging;
using SharedKernel;

namespace Bike4Me.Application.Couriers.Commands;

public sealed class ImportCourierCnhCommand(Guid courierId, string imagemCnh) : Command<Result<Guid>>
{
    public Guid CourierId => courierId;
    public string ImagemCnh => imagemCnh;
}