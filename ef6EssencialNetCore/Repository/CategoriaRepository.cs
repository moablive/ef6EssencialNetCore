using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Repository;

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context){}

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(
                c => c.Produtos
            );
        }
    }
