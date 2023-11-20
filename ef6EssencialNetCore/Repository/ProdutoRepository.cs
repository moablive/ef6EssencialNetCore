using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context){}

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy (
                p => p.Preco
            ).ToList();
        }
    }
