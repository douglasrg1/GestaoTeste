using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class MovimentaEstoque
    {
        [Key]
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
        [Display(Name ="Fornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

    }
}