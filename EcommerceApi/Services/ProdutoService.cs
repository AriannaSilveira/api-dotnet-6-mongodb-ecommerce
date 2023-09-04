using AutoMapper;
using EcommerceApi.Data;
using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EcommerceApi.Services;

public class ProdutoService
{
    private ProdutoContext _context;
    private IMapper _mapper;

    public ProdutoService(ProdutoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Produto> CreateProduto(CreateProdutoDto produtoDto)
    {
        var produto = _mapper.Map<Produto>(produtoDto);

        await _context.Produtos.InsertOneAsync(produto);
        return produto;
    }

    public async Task<ReadProdutoDto> GetProdutoById(string id)
    {
        var constructor = Builders<Produto>.Filter;
        var condition = constructor.Eq(x => x.Id, id);

        var produto = await _context.Produtos.Find(condition).FirstOrDefaultAsync();

        if (produto == null)
            return null;

        return _mapper.Map<ReadProdutoDto>(produto);
    }

    public async Task<List<ReadProdutoDto>> GetProdutosList(string? nome, float? precoMaiorQue, float? precoMenorQue, int pagina, string? ordem)
    {
        var constructor = Builders<Produto>.Filter;
        var condition = constructor.Empty;

        if (!string.IsNullOrEmpty(nome))
        {
            condition &= constructor.Regex(c => c.Nome, new BsonRegularExpression(nome, "i"));
        }

        if (precoMaiorQue.HasValue)
        {
            condition &= constructor.Gt(c => c.Valor, precoMaiorQue.Value);
        }

        if (precoMenorQue.HasValue)
        {
            condition &= constructor.Lt(c => c.Valor, precoMenorQue.Value);
        }

        int take = 10;
        int quantPorPag = pagina * take;

        var query = _context.Produtos.Find(condition);

        if (!string.IsNullOrEmpty(ordem))
        {
            if (ordem == "asc")
            {
                query = query.SortBy(p => p.Valor);
            }
            else if (ordem == "desc")
            {
                query = query.SortByDescending(p => p.Valor);
            }
            else
            {
                throw new ArgumentException("Valor de ordem inválido.");
            }
        }

        var produtos = await query.Skip(quantPorPag).Limit(take).ToListAsync();
        return _mapper.Map<List<ReadProdutoDto>>(produtos);
    }

    public async Task<bool> DeleteProduto(string id)
    {
        var produto = await GetProdutoById(id);
        if (produto == null)
            return false;

        var deleteResult = await _context.Produtos.DeleteOneAsync(c => c.Id == id);
        return deleteResult.DeletedCount > 0;
    }
}
