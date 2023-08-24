using EcommerceApi.Models;
using MongoDB.Driver;

namespace EcommerceApi.Data;

public class ProdutoContext
{
    private readonly IMongoDatabase _database;

    public ProdutoContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("ProdutoConnection"));
        _database = client.GetDatabase("Ecommerce");
    }

    public IMongoCollection<Produto> Produtos => _database.GetCollection<Produto>("Produtos");
}
