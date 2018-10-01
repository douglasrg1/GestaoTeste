using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Produto
    {
        public int id { get; set; }
        [Required]
        [MaxLength(100)]
        [Index("nome",IsUnique =true)]
        [Display(Name ="Nome")]
        public string nome { get; set; }
        [Required]
        [Display(Name ="Valor de compra")]
        public decimal valorCompra { get; set; }
        [Display(Name ="Valor do frete")]
        public decimal? valorFrete { get; set; }
        [Display(Name ="Despesas adicionais da compra")]
        public decimal? despesasAdicionaisdeCompra { get; set; }
        [Required]
        [Display(Name ="Valor de venda")]
        public decimal valorVenda { get; set; }
        [Display(Name ="Despesas adicionais de venda")]
        public decimal? despesasAdicionaisdeVenda { get; set; }
        [Display(Name ="Data de cadastro")]
        public DateTime dataCadastro { get; set; }
        [Display(Name ="Data da ultima entrada")]
        public DateTime? dataUltimaEntrada { get; set; }
        [Display(Name ="Data da ultima saída")]
        public DateTime? dataUltimaSaida { get; set; }
        [Display(Name ="Desconto")]
        public decimal? desconto { get; set; }
        [MaxLength(100)]
        [Display(Name ="Código de referência")]
        public string condigoReferencia { get; set; }
        [Display(Name ="Quantidade em estoque")]
        public decimal quantidadeEstoque { get; set; }
        [MaxLength(250)]
        [Display(Name ="Observações")]
        public string observacoes { get; set; }
    }
}