using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Data.Dtos;

public class CreateProdutoDto
{
    [Required(ErrorMessage = "Por gentileza, digite o nome do produto.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Por gentileza, digite ovalor do produto.")]
    public float Valor { get; set; }

    [Required(ErrorMessage = "Por gentileza, digite a descrição do produto.")]
    public string Descricao { get; set; }
}
