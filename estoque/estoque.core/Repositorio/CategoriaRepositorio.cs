using estoque.core.Categoria.Model;
using Microsoft.EntityFrameworkCore;

namespace estoque.core.Categoria.Repositorio
{
    public class CategoriaRepositorio(CategoriaContext categoriaContext)
    {
        private readonly CategoriaContext _categoriaContext = categoriaContext;

        public async Task<List<CategoriaModel>> ObterCategoriasAsync()
        {
            return await _categoriaContext.Categorias.ToListAsync();
        }

        public async Task<CategoriaModel?> ObterCategoriaPorIdAsync(Guid id)
        {
            return await _categoriaContext.Categorias.FindAsync(id);
        }
        public async Task AdicionarCategoriaAsync(CategoriaModel categoria)
        {
            await _categoriaContext.Categorias.AddAsync(categoria);
            await _categoriaContext.SaveChangesAsync();
        }
        public async Task AtualizarCategoriaAsync(CategoriaModel categoria)
        {
            _categoriaContext.Categorias.Update(categoria);
            await _categoriaContext.SaveChangesAsync();
        }
        public async Task RemoverCategoriaAsync(Guid id)
        {
            var categoria = await _categoriaContext.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _categoriaContext.Categorias.Remove(categoria);
                await _categoriaContext.SaveChangesAsync();
            }
        }

    }
}
