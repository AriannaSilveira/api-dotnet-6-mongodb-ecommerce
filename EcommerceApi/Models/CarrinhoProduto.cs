using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models;

public class CarrinhoProduto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public float PrecoProduto { get; set; }
}
