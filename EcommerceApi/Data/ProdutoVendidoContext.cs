using EcommerceApi.Models;
using MongoDB.Driver;

namespace EcommerceApi.Data;

public class ProdutoVendidoContext
{
    private readonly IMongoDatabase _database;

    public ProdutoVendidoContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("ProdutoConnection"));
        _database = client.GetDatabase("Ecommerce");
    }

    public IMongoCollection<ProdutoVendido> ProdutosVendidos => _database.GetCollection<ProdutoVendido>("ProdutosVendidos");
}
