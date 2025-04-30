using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bike4Me.Domain.Bikes;

public class BikeReport
{
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("plate")]
    public string Plate { get; set; } = string.Empty;

    [BsonElement("modelName")]
    public string ModelName { get; set; } = string.Empty;

    [BsonElement("year")]
    public int Year { get; set; }
}