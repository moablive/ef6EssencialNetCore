using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
