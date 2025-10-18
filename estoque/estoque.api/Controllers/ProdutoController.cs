using estoque.core.Produto.Model;
using estoque.core.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace estoque.api.Controllers
{
    [ApiController]
    [Route("produtos")]
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoModel>>> GetAll()
        {
            var produtos = await _produtoRepositorio.ObterProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoModel>> GetById(Guid id)
        {
            var produto = await _produtoRepositorio.ObterProdutoPorIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoModel>> Create([FromBody] ProdutoModel produto)
        {
            produto.Id = produto.Id == Guid.Empty ? Guid.NewGuid() : produto.Id;
            await _produtoRepositorio.AdicionarProdutoAsync(produto);
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        // PUT: api/produto/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProdutoModel produto)
        {
            if (id != produto.Id) return BadRequest("O id do recurso e do payload devem ser iguais.");

            var existente = await _produtoRepositorio.ObterProdutoPorIdAsync(id);
            if (existente == null) return NotFound();

            // Opcional: copiar campos necessários — aqui atualizamos diretamente
            await _produtoRepositorio.AtualizarProdutoAsync(produto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existente = await _produtoRepositorio.ObterProdutoPorIdAsync(id);
            if (existente == null) return NotFound();

            await _produtoRepositorio.RemoverProdutoAsync(id);
            return NoContent();
        }
    }
}
