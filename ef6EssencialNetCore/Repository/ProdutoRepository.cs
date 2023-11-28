using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Repository;

    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context){}
        
        public async Task<PagedList<Produto>> GetProdutos(ProdutoParameters produtoParameters)
        {
            return await PagedList<Produto>.ToPagedList(
                    Get().OrderBy(p => p.ProdutoId),
                    produtoParameters.PageNumber,
                    produtoParameters.PageSize
                );   
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy (
                p => p.Preco
            ).ToListAsync();
        }
    }
