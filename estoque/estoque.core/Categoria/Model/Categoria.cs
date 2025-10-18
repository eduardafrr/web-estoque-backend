using System;

namespace estoque.core.Categoria.Model
{
    public class CategoriaModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}