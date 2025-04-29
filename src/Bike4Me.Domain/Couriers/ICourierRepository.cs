namespace Bike4Me.Domain.Couriers;

public interface ICourierRepository
{
    Task<bool> ExistsByCnpjAsync(string cnpj);

    Task AddAsync(Courier courier);
}