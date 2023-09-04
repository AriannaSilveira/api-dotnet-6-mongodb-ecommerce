namespace EcommerceApi.Data.Dtos;

public class DetalhesCarrinhoDto
{
    public List<ReadCarrinhoProdutoDto> Produtos { get; set; }
    public float Total { get; set; }
}
