using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CarrinhoController : ControllerBase
{
    private CarrinhoService _carrinhoService;
    private ProdutoService _produtoService;
    private ProdutoVendidoService _produtosVendidosService;

    public CarrinhoController(CarrinhoService carrinhoService, ProdutoService produtoService, ProdutoVendidoService produtosVendidosService)
    {
        _carrinhoService = carrinhoService;
        _produtoService = produtoService;
        _produtosVendidosService = produtosVendidosService;
    }

    [HttpPost("adicionarproduto")]
    public async Task<IActionResult> AddProdutoCarrinho([FromBody] AddProdutoCarrinhoDto dto)
    {
        var resultadoDoServico = await _carrinhoService.AddProdutoCarrinho(dto);

        if (resultadoDoServico is NotFoundResult)
            return NotFound(new { mensagem = "Produto não encontrado no serviço." });
        

        if (resultadoDoServico is OkObjectResult)
            return Ok(new { mensagem = "Produto adicionado ao carrinho com sucesso." });

        return BadRequest(new { mensagem = "Erro desconhecido." });
    }

    [HttpGet("detalhes")]
    public async Task<IActionResult> GetDetalhesCarrinho()
    {
        var carrinhoDetalhes = await _carrinhoService.GetDetalhesCarrinho();

        if (carrinhoDetalhes == null)
            return NotFound();
        
        return Ok(carrinhoDetalhes);
    }

    [HttpPost("compraritens")]
    public async Task<IActionResult> ComprarProdutosCarrinho()
    {
        var itensCarrinho = await _carrinhoService.GetDetalhesCarrinho();

        if (itensCarrinho.Produtos.Count == 0)
            return BadRequest(new { mensagem = "Nenhum item no carrinho para compra." });
        

        var itensVendidos = new List<ProdutoVendido>();

        foreach (var produto in itensCarrinho.Produtos)
        {
            var produtoVendido = new ProdutoVendido
            {
                ProdutoId = produto.ProdutoId,
                NomeProduto = produto.NomeProduto,
                PrecoProduto = produto.PrecoProduto,
                DataVenda = DateTime.UtcNow
            };

            itensVendidos.Add(produtoVendido);
        }

        await _produtosVendidosService.AddProdutosVendidos(itensVendidos);

        await _carrinhoService.RemoverProdutosCarrinho();
        
        foreach (var produto in itensCarrinho.Produtos)
        {
            await _produtoService.DeleteProduto(produto.ProdutoId);
        }

        return Ok(new { mensagem = "Todos os itens do carrinho foram comprados com sucesso." });
    }
}