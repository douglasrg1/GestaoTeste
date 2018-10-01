using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models.Adicionais
{
    public class Estados
    {
        public int Id { get; set; }
        [Required]
        public int CodigoUf { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(2)]
        public string Uf { get; set; }
        [Required]
        public int Regiao { get; set; }
    }
}