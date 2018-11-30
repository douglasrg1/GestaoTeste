using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }
        [Required]
        [Display(Name ="CNPJ")]
        public string cnpj { get; set; }

        [Required]
        [Display(Name ="Razão Social")]
        public string razaoSocial { get; set; }

        [Required]
        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [Required]
        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Required]
        [Display(Name = "Numero")]
        public string numero { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Primeiro telefone para contato")]
        public string telefoneContato1 { get; set; }

        [Display(Name = "Segundo telefone para contato")]
        public string telefoneContato2 { get; set; }
        
        [Required]
        [Display(Name ="Senha")]
        [MinLength(8,ErrorMessage = "O campo senha deve conter pelo menos 8 caracteres")]
        public string senha { get; set; }

    }
}