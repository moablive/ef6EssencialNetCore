using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutoParameters produtoParameters);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
