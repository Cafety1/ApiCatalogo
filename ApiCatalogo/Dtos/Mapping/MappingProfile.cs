using ApiCatalogo.Models;
using AutoMapper;

namespace ApiCatalogo.Dtos.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto,ProdutoDto>().ReverseMap();
            CreateMap<Categoria,CategoriaDto>().ReverseMap();
        }
    }
}
