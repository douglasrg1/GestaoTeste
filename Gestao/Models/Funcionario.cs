using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Funcionario
    {
        public int id { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name ="Nome")]
        public string nome { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name = "Cpf")]
        public string cpfcnpj {get;set;}
        [Display(Name ="Salário")]
        public decimal? salario { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Rua")]
        public string rua { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Número")]
        public string numero { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Bairro")]
        public string bairro { get; set; }
        [Required]
        [MaxLength(2)]
        [Display(Name = "Estado")]
        public string estado { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string telefone1 { get; set; }
        [MaxLength(20)]
        [Display(Name = "Telefone 2")]
        public string telefone2 { get; set; }
        [MaxLength(100)]
        [Display(Name = "E-mail")]
        public string email { get; set; }
        public DateTime dataEmissao { get; set; }
        public DateTime? dataDemissao { get; set; }

    }
}