using Gestao.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.ViewModel
{
    public class MovimentaEstoqueViewModel
    {
        public int idMovimentacao { get; set; }
        [Display(Name = "Data da Movimentação")]
        public DateTime datamovimentacao { get; set; }
        [Display(Name = "Tipo de Movimentação")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(20)]
        public string tipoMovimentacao { get; set; }
        [Display(Name = "Numero do documento")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(50)]
        public string nrDocumento { get; set; }
        [Display(Name = "Total da Movimentação")]
        public decimal? totalMovimentacao { get; set; }
        [Display(Name = "Valor do frete")]
        public decimal? valorFrete { get; set; }
        [Display(Name = "Valor ipi")]
        public decimal? valorIpi { get; set; }
        [Display(Name = "Valor icms")]
        public decimal? valorIcms { get; set; }
        [Display(Name = "CFOP")]
        public string cfop { get; set; }
        [ForeignKey("Fornecedor")]
        public int? idFornecedor { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Fornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

        public int idProdutosMovimentacao { get; set; }

        [ForeignKey("MovimentaEstoque")]
        [Required]
        public int idMovimentacaoEstoque { get; set; }
        public virtual MovimentaEstoque MovimentaEstoque { get; set; }

        [ForeignKey("Produto")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int idProduto { get; set; }
        public virtual Produto Produto { get; set; }

        public decimal quantidade { get; set; }
        public decimal valorUnitario { get; set; }
        public decimal valorTotal { get; set; }
        public decimal? aliqICMS { get; set; }
        public decimal? CFOP { get; set; }
        public decimal? valorICMS { get; set; }
        public string cst_cson { get; set; }
        public decimal? aliqIPI { get; set; }
        public decimal? valorIPI { get; set; }
        public decimal? valorBaseICMSST { get; set; }
        public decimal? valorICMSST { get; set; }
        public decimal? valorDesconto { get; set; }
    }
}