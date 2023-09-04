using EcommerceApi.Models;
using MongoDB.Driver;

namespace EcommerceApi.Data;

public class CarrinhoContext
{
    private readonly IMongoDatabase _database;

    public CarrinhoContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("ProdutoConnection"));
        _database = client.GetDatabase("Ecommerce");
    }

    public IMongoCollection<CarrinhoProduto> Carrinho => _database.GetCollection<CarrinhoProduto>("Carrinho");
}
