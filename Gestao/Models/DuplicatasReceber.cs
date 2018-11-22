using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class DuplicatasReceber
    {
        [Key]
        public int idDuplicataReceber { get; set; }
        public string numeroDuplicata { get; set; }
        [ForeignKey("Cliente")]
        public int idCliente { get; set; }
        [Required]
        [Display(Name = "Cliente")]
        public virtual Cliente Cliente { get; set; }
        [Required]
        [Display(Name = "Data Hemissão")]
        public DateTime dataHemissao { get; set; }
        [Required]
        [Display(Name = "Data Vencimento")]
        public DateTime dataVencimento { get; set; }
        [Display(Name = "Data Pagamento")]
        public DateTime? dataPagamento { get; set; }
        [Display(Name = "Valor Duplicata")]
        public decimal valorDuplicata { get; set; }
        [Display(Name = "Valor Desconto")]
        public decimal valorDesconto { get; set; }
        [Display(Name = "Valor Pago")]
        public decimal valorPago { get; set; }
        [Display(Name = "Valor Devedor")]
        public decimal valorDevedor { get; set; }
        [Display(Name = "Valor Multa")]
        public decimal valorMulta { get; set; }
        [Display(Name = "Juros por dia de atraso")]
        public decimal valorJurosPorDia { get; set; }
        [Display(Name = "Observação")]
        public string observacao { get; set; }
        [Display(Name = "Status da duplicata")]
        public string statusDuplicata { get; set; }
        [ForeignKey("Pedido")]
        public int idPedido { get; set; }
    }
}