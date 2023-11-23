using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutoParameters produtoParameters);
        IEnumerable<Produto> GetProdutosPorPreco();
    }
