using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Commands;

public record UpdateMotorcyclePlateCommand(Guid Id, string Plate) : IRequest<Result>;