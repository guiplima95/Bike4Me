using Bike4Me.Domain.Bikes;
using MongoDB.Driver;

namespace Bike4Me.Infrastructure.Repositories;

public sealed class BikeReportRepository(IMongoCollection<BikeReport> collection) : IBikeReportRepository
{
    public async Task UpsertAsync(BikeReport bike)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, bike.Id);
        var options = new ReplaceOptions { IsUpsert = true };
        await collection.ReplaceOneAsync(filter, bike, options);
    }

    public async Task<BikeReport?> GetByIdAsync(Guid id)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, id.ToString());
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<BikeReport?> GetByPlateAsync(string plate)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Plate, plate);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<BikeReport>> GetAllAsync()
    {
        return await collection.Find(Builders<BikeReport>.Filter.Empty).ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, id.ToString());
        await collection.DeleteOneAsync(filter);
    }
}