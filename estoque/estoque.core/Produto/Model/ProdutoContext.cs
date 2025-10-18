using Microsoft.EntityFrameworkCore;

namespace estoque.core.Produto.Model
{
    public class ProdutoContext : DbContext
    {
        public ProdutoContext(DbContextOptions<ProdutoContext> options)
            : base(options)
        {
        }

        public DbSet<ProdutoModel> Produtos { get; set; } = null!;
    }
}

