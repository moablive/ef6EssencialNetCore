using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public interface ICategoriaRepository : IRepository<Categoria>
    {   
        PagedList<Categoria> GetCategorias(CategoriaParameter categoriaParameter);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
