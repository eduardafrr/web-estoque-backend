using estoque.core.Categoria.Model;
using estoque.core.Categoria.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace estoque.api.Controllers
{
    [ApiController]
    [Route("categorias")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaRepositorio _categoriaRepositorio;

        public CategoriaController(CategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaModel>>> GetAll()
        {
            var categorias = await _categoriaRepositorio.ObterCategoriasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoriaModel>> GetById(Guid id)
        {
            var categoria = await _categoriaRepositorio.ObterCategoriaPorIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaModel>> Create([FromBody] CategoriaModel categoria)
        {
            categoria.Id = categoria.Id == Guid.Empty ? Guid.NewGuid() : categoria.Id;
            await _categoriaRepositorio.AdicionarCategoriaAsync(categoria);
            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoriaModel categoria)
        {
            if (id != categoria.Id) return BadRequest("O id do recurso e do payload devem ser iguais.");

            var existente = await _categoriaRepositorio.ObterCategoriaPorIdAsync(id);
            if (existente == null) return NotFound();

            await _categoriaRepositorio.AtualizarCategoriaAsync(categoria);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existente = await _categoriaRepositorio.ObterCategoriaPorIdAsync(id);
            if (existente == null) return NotFound();

            await _categoriaRepositorio.RemoverCategoriaAsync(id);
            return NoContent();
        }
    }
}
