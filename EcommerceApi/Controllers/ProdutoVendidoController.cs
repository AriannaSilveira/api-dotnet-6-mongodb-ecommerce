using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoVendidoController : ControllerBase
{
    private ProdutoVendidoService _produtoVendidoService;

    public ProdutoVendidoController(ProdutoVendidoService produtoVendidoService)
    {
        _produtoVendidoService = produtoVendidoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutosVendidosPorMes(int? mes)
    {
        var produtosVendidos = await _produtoVendidoService.GetProdutosVendidosPorMes(mes);

        if(produtosVendidos == null)
            return NotFound();

        return Ok(produtosVendidos);
    }

}