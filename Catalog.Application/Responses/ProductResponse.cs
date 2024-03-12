using Catalog.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Responses;

public class ProductResponse
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageFile { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductBrand Brands { get; set; } = new();
    public ProductType Types { get; set; } = new();
}
