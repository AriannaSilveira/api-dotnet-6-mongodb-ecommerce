using EcommerceApi.Data;
using EcommerceApi.Models;
using MongoDB.Driver;

namespace EcommerceApi.Services;

public class ProdutoVendidoService
{
    private ProdutoVendidoContext _context;

    public ProdutoVendidoService(ProdutoVendidoContext context)
    {
        _context = context;
    }

    public async Task AddProdutosVendidos(List<ProdutoVendido> itensVendidos)
    {
        foreach (var itemVendido in itensVendidos)
        {
            await _context.ProdutosVendidos.InsertOneAsync(itemVendido);
        }
    }

    public async Task<List<ProdutoVendido>> GetProdutosVendidosPorMes(int? mes)
    {
        var filtro = Builders<ProdutoVendido>.Filter;

        if (mes.HasValue)
        {
            var filtroPorMes = filtro.Where(x => x.DataVenda.Month == mes);
            return await _context.ProdutosVendidos.Find(filtroPorMes).ToListAsync();
        }
        else
        {
            return await _context.ProdutosVendidos.Find(_ => true).ToListAsync();
        }
    }

}
