using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context){}
        
        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return PagedList<Produto>.ToPagedList(
                Get().OrderBy(
                    p => p.ProdutoId
                ),produtosParameters.PageNumber,produtosParameters.PageSize
            );
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy (
                p => p.Preco
            ).ToList();
        }
    }
