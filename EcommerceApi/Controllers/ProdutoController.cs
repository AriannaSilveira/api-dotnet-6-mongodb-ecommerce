using EcommerceApi.Data.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutoController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoDto produtoDto)
    {
        var produto = await _produtoService.CreateProduto(produtoDto);
        return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdutoById(string id)
    {
        var produtoDto = await _produtoService.GetProdutoById(id);

        if (produtoDto == null)
            return NotFound();

        return Ok(produtoDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutosList([FromQuery] string? nome = null, [FromQuery] float? precoMaiorQue = null, [FromQuery] float? precoMenorQue = null, [FromQuery] int pagina = 0, [FromQuery] string? ordem = null)
    {
        var produtosDto = await _produtoService.GetProdutosList(nome, precoMaiorQue, precoMenorQue, pagina, ordem);

        return Ok(produtosDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(string id)
    {
        var produto = await _produtoService.GetProdutoById(id);

        if (produto == null)
            return NotFound();
        

        var deleted = await _produtoService.DeleteProduto(id);

        if (deleted)
            return NoContent();
        
        return BadRequest("Erro ao excluir o produto.");
    }
}