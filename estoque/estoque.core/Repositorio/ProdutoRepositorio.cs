using estoque.core.Produto.Model;
using Microsoft.EntityFrameworkCore;

namespace estoque.core.Repositorio
{
    public class ProdutoRepositorio(ProdutoContext produtoContext)
    {
        private readonly ProdutoContext _produtoContext = produtoContext;
        public async Task<List<ProdutoModel>> ObterProdutosAsync()
        {
            return await _produtoContext.Produtos.ToListAsync();
        }
        public async Task<ProdutoModel?> ObterProdutoPorIdAsync(Guid id)
        {
            return await _produtoContext.Produtos.FindAsync(id);
        }
        public async Task AdicionarProdutoAsync(ProdutoModel produto)
        {
            await _produtoContext.Produtos.AddAsync(produto);
            await _produtoContext.SaveChangesAsync();
        }
        public async Task AtualizarProdutoAsync(ProdutoModel produto)
        {
            _produtoContext.Produtos.Update(produto);
            await _produtoContext.SaveChangesAsync();
        }
        public async Task RemoverProdutoAsync(Guid id)
        {
            var produto = await _produtoContext.Produtos.FindAsync(id);
            if (produto != null)
            {
                _produtoContext.Produtos.Remove(produto);
                await _produtoContext.SaveChangesAsync();
            }
        }
    }
}
