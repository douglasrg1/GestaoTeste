using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Campo tipo de cliente é obrigatório")]
        [MaxLength(10)]
        [Display(Name ="Tipo de Cliente")]
        public string TipoCliente { get; set; }
        [MaxLength(20)]
        [Display(Name ="Cpf/Cnpj")]
        public string CnpjCpf { get; set; }
        [Required(ErrorMessage = "O campo Rua é obrigatório")]
        [MaxLength(100)]
        public string Rua { get; set; }
        [Required(ErrorMessage = "O campo numero é obrigatório")]
        [MaxLength(15)]
        [Display(Name = "Número")]
        public string Numero { get; set; }
        [Required(ErrorMessage ="O campo bairo é obrigatório")]
        [MaxLength(100)]
        public string Bairro { get; set; }
        [Required(ErrorMessage ="O campo cidade é obrigatório")]
        [MaxLength(100)]
        [Display(Name ="Cidade")]
        public string cidade { get; set; }
        [Required(ErrorMessage ="O campo estado é obrigatório")]
        [MaxLength(2)]
        public string Estado { get; set; }
        [Required(ErrorMessage ="O campo telefone é obrigatório")]
        [MaxLength(20)]
        [Display(Name ="Telefone")]
        public string Telefone1 { get; set; }
        [MaxLength(20)]
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        [Display(Name ="Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
        [Display(Name ="Data da Ultima Compra")]
        public DateTime? DataUtimaCompra { get; set; }
    }
}