using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Repository;

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context){}

        public PagedList<Categoria> GetCategorias(CategoriaParameter categoriaParameter)
        {
            return PagedList<Categoria>.ToPagedList(
                Get().OrderBy(
                    c => c.Nome
                ),categoriaParameter.PageNumber,categoriaParameter.PageSize
            );
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(
                c => c.Produtos
            );
        }
    }
