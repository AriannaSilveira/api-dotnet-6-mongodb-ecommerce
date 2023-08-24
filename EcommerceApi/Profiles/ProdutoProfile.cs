using AutoMapper;
using EcommerceApi.Data.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Profiles;

public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<CreateProdutoDto, Produto>();
        CreateMap<Produto, ReadProdutoDto>();
    }
}
