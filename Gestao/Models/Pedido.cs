using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Pedido
    {
        public int id { get; set; }
        [ForeignKey("Cliente")]
        public int idCliente { get; set; }
        [Required]
        [Display(Name ="Cliente")]
        public virtual Cliente Cliente { get; set; }
        public DateTime dataPedido { get; set; }
        [Display(Name ="Valor Comissão")]
        public decimal? valorComissao { get; set; }
        [MaxLength(150)]
        [Display(Name ="Observação")]
        public string observacao { get; set; }
        [Display(Name ="Total do pedido")]
        public decimal totalPedido { get; set; }
        [Display(Name ="Total Pago")]
        public decimal totalPago { get; set; }
        [Display(Name ="Total Devedor")]
        public decimal valorDevedor { get; set; }
        public List<ProdutoPedido> ProdutoPedido { get; set; }
        [Display(Name ="Numero do Pedido")]
        [Index(IsUnique = true)]
        public int numeroPedido { get; set; }

    }
}