using Microsoft.EntityFrameworkCore;

namespace estoque.core.Categoria.Model
{
    public class CategoriaContext : DbContext
    {
        public CategoriaContext(DbContextOptions<CategoriaContext> options)
            : base(options)
        {
        }

        public DbSet<CategoriaModel> Categorias { get; set; } = null!;
    }
}
