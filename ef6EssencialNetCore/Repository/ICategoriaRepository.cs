using ef6EssencialNetCore.Models;

namespace ef6EssencialNetCore.Repository;

    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
