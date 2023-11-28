using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Repository;

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context){}

        public async Task<PagedList<Categoria>> GetCategorias(CategoriaParameter categoriaParameter)
        {
            return await PagedList<Categoria>.ToPagedList(
                    Get().OrderBy(c => c.Nome),
                    categoriaParameter.PageNumber,
                    categoriaParameter.PageSize
                );
        }
        
        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(
                c => c.Produtos
            ).ToListAsync();
        }
    }
