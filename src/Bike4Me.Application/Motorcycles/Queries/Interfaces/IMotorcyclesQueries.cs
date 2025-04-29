using Bike4Me.Application.Motorcycles.Dtos;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Queries.Interfaces;

public interface IMotorcyclesQueries
{
    Task<Result<List<MotorcycleDto>>> GetAllAsync();

    Task<Result<MotorcycleDto>> GetByPlateAsync(string plate);
}