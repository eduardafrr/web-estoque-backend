using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estoque.core.Produto.Model
{
    public class ProdutoModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }
        public int Quantidade { get; set; }
        public int? QuantidadeMinima { get; set; }
        public Guid CategoriaId { get; set; }
    }

    public enum UnidadeMedida
    {
        Unidade,
        Quilograma,
        Litro,
        Pacote,
        Caixa,
        Metro
    }
}
