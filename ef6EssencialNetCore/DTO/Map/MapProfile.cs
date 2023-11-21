using AutoMapper;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.DTO.Map;

    public class MapProfile :Profile
    {
        public MapProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
