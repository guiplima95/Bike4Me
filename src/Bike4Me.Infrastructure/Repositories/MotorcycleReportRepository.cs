using Bike4Me.Domain.Motorcycles;
using MongoDB.Driver;

namespace Bike4Me.Infrastructure.Repositories;

public sealed class MotorcycleReportRepository(IMongoCollection<MotorcycleReport> collection) : IMotorcycleReportRepository
{
    public async Task UpsertAsync(MotorcycleReport motorcycle)
    {
        var filter = Builders<MotorcycleReport>.Filter.Eq(m => m.Id, motorcycle.Id);
        var options = new ReplaceOptions { IsUpsert = true };
        await collection.ReplaceOneAsync(filter, motorcycle, options);
    }

    public async Task<MotorcycleReport?> GetByIdAsync(Guid id)
    {
        var filter = Builders<MotorcycleReport>.Filter.Eq(m => m.Id, id.ToString());
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<MotorcycleReport?> GetByPlateAsync(string plate)
    {
        var filter = Builders<MotorcycleReport>.Filter.Eq(m => m.Plate, plate);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<MotorcycleReport>> GetAllAsync()
    {
        return await collection.Find(Builders<MotorcycleReport>.Filter.Empty).ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<MotorcycleReport>.Filter.Eq(m => m.Id, id.ToString());
        await collection.DeleteOneAsync(filter);
    }
}