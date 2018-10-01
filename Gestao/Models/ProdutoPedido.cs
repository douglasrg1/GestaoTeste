using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class ProdutoPedido
    {
        public int id { get; set; }
        public Produto Produto { get; set; }
        public Pedido Pedido { get; set; }
        public decimal valorUnitario { get; set; }
        public decimal? porcDesconto { get; set; }
        public decimal? valorDesconto { get; set; }
        public decimal quantidade { get; set; }
        public decimal valorTotal { get; set; }
        [MaxLength(200)]
        public string observacao { get; set; }

    }
}