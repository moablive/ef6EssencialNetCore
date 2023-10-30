using ef6EssencialNetCore.Models;
using Microsoft.EntityFrameworkCore;
namespace ef6EssencialNetCore.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base( options ) {}

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
}
