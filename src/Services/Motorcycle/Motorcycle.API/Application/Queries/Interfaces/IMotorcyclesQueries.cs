using Motorcycle.API.Application.Dtos;
using Motorcycle.API.Domain.MotorcycleAggregate;

namespace Motorcycle.API.Application.Queries.Interfaces;

public interface IMotorcyclesQueries
{
    Task<List<MotorcycleDto>> GetAllMotorcyclesAsync();
}