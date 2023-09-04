using AutoMapper;
using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Profiles;

public class CarrinhoProfile : Profile
{
    public CarrinhoProfile()
    {
        CreateMap<CarrinhoProduto, AddProdutoCarrinhoDto>();
        CreateMap<CarrinhoProduto, ReadCarrinhoProdutoDto>();
    }
}
