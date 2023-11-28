using ef6EssencialNetCore.Context;

namespace ef6EssencialNetCore.Repository;

    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository;
        public AppDbContext _Context;

        
        public UnitOfWork(AppDbContext context)
        {
            _Context = context;
        }
        

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_Context); 
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_Context); 
            }
        }

        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }