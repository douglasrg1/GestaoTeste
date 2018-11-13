using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class ProdutosMovimentacao
    {
        [Key]
        public int idProdutosMovimentacao { get; set; }

        [ForeignKey("MovimentaEstoque")]
        [Required]
        public int idMovimentacaoEstoque { get; set; }
        public virtual MovimentaEstoque MovimentaEstoque { get; set; }

        [ForeignKey("Produto")]
        [Required(ErrorMessage ="Campo Obrigatório")]
        public int idProduto { get; set; }
        public virtual Produto Produto { get; set; }

        public decimal quantidade { get; set; }
        public decimal valorUnitario { get; set; }
        public decimal valorTotal { get; set; }
        public decimal? aliqICMS { get; set; }
        public string CFOP { get; set; }
        public decimal? valorICMS { get; set; }
        public string cst_cson { get; set; }
        public decimal? aliqIPI { get; set; }
        public decimal? valorIPI { get; set; }
        public decimal? valorBaseICMSST { get; set; }
        public decimal? valorICMSST { get; set; }
        public decimal? valorDesconto { get; set; }
        public string observacao { get; set; }
        public decimal? porcdesconto { get; set; }
    }
}