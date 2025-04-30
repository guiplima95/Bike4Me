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

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, id);
        await collection.DeleteOneAsync(filter);
    }

    public async Task<BikeReport?> GetByIdAsync(string id)
    {
        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}