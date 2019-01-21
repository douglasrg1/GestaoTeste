using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Caixa
    {
        [Key]
        public int CaixaId { get; set; }
        [MaxLength(10)]
        [Required(ErrorMessage ="Campo obrigatório")]
        [Display(Name ="Tipo de movimentação")]
        public string TipoMovimentacao { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        [Display(Name ="Valor da movimentação")]
        public decimal ValorMovimentacao { get; set; }
        [Required]
        public DateTime DataMovimentacao { get; set; }
        [Display(Name ="Observação")]
        [MaxLength(100)]
        public string descricao { get; set; }
        [ForeignKey("Duplicata")]
        public int? DuplicataId { get; set; }
        public virtual DuplicatasReceber Duplicata { get; set; }
        [ForeignKey("Pedido")]
        public int? PedidoId { get; set; }
        public virtual Pedido Pedido { get; set; }
        [ForeignKey("Funcionario")]
        public int? FuncionarioId { get; set; }
        public virtual Funcionario Funcionario { get; set; }

    }
}