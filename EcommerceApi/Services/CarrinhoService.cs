using AutoMapper;
using EcommerceApi.Data;
using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcommerceApi.Services;

public class CarrinhoService
{
    private CarrinhoContext _context;
    private ProdutoService _produtoService;
    private IMapper _mapper;

    public CarrinhoService(CarrinhoContext context, ProdutoService produtoService, IMapper mapper)
    {
        _context = context;
        _produtoService = produtoService;
        _mapper = mapper;
    }

    public async Task<IActionResult> AddProdutoCarrinho([FromBody] AddProdutoCarrinhoDto dto)
    {
        var produto = await _produtoService.GetProdutoById(dto.ProdutoId);

        if (produto == null)
            return new NotFoundResult();
        
        var produtoAoCarrinho = new CarrinhoProduto
        {
            ProdutoId = produto.Id,
            NomeProduto = produto.Nome,
            PrecoProduto = produto.Valor
        };

        await _context.Carrinho.InsertOneAsync(produtoAoCarrinho);

        return new OkObjectResult(new { mensagem = "Produto adicionado ao carrinho com sucesso." });
    }

    public async Task<DetalhesCarrinhoDto> GetDetalhesCarrinho()
    {

        var produtos = await _context.Carrinho.Find(_ => true).ToListAsync();
        var produtosDto = _mapper.Map<List<ReadCarrinhoProdutoDto>>(produtos);
        var total = produtos.Sum(p => p.PrecoProduto); 

        var carrinhoDetalhes = new DetalhesCarrinhoDto
        {
            Produtos = produtosDto,
            Total = total
        };

        return carrinhoDetalhes;
    }

    public async Task RemoverProdutosCarrinho()
    {
        await _context.Carrinho.DeleteManyAsync(_ => true);
    }
}
