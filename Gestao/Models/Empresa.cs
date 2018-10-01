using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Empresa
    {
        public int id { get; set; }
        [Required]
        [Display(Name ="Razão Social")]
        [MaxLength(100)]
        public string razaoSocial { get; set; }
        [Required]
        [Display(Name = "Nome Fantasia")]
        [MaxLength(100)]
        public string nomeFantasia { get; set; }
        [Required]
        [Display(Name = "CNPJ")]
        [MaxLength(30)]
        public string cnpj { get; set; }
        [Display(Name = "Inscrição Estadual")]
        [MaxLength(30)]
        public string inscricaoEstadual { get; set; }
        [Display(Name = "Inscrição Municipal")]
        [MaxLength(30)]
        public string inscricaoMunicipal { get; set; }
        [Required]
        [Display(Name = "Rua")]
        [MaxLength(100)]
        public string rua { get; set; }
        [Required]
        [Display(Name = "Numero")]
        [MaxLength(20)]
        public string numero { get; set; }
        [Required]
        [Display(Name = "Bairro")]
        [MaxLength(100)]
        public string bairro { get; set; }
        [Display(Name ="Complemento")]
        [MaxLength(100)]
        public string complemento { get; set; }
        [Required]
        [Display(Name = "Estado")]
        [MaxLength(2)]
        public string estado { get; set; }
        [Required]
        [Display(Name = "Cidade")]
        [MaxLength(100)]
        public string cidade { get; set; }
        [Required]
        [Display(Name = "Telefone")]
        public string telefone1 { get; set; }
        public string telefone2 { get; set; }
        public string email { get; set; }
    }
}