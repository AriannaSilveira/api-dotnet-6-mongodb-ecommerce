using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using EcommerceApi.Data;
using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private ProdutoContext _context;
    private IMapper _mapper;
    public ProdutoController(ProdutoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoDto produtooDto)
    {
        Produto produto = _mapper.Map<Produto>(produtooDto);

        await _context.Produtos.InsertOneAsync(produto);
        return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdutoById(string id)
    {
        var constructor = Builders<Produto>.Filter;
        var condition = constructor.Eq(x => x.Id, id);

        var produto = await _context.Produtos.Find(condition).FirstOrDefaultAsync();

        if (produto == null) return NotFound();
        var produtoDto = _mapper.Map<ReadProdutoDto>(produto);
        return Ok(produtoDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutosList([FromQuery] string? nome = null, [FromQuery] float? precoMaiorQue = null, [FromQuery] float? precoMenorQue = null)
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

        var produtos = await _context.Produtos.Find(condition).ToListAsync();
        var produtosDto = _mapper.Map<List<ReadProdutoDto>>(produtos);

        return Ok(produtosDto);
    }
}