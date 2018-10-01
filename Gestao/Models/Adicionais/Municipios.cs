using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models.Adicionais
{
    public class Municipios
    {
        public int Id { get; set; }
        [Required]
        public int Codigo { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(2)]
        public string Uf { get; set; }
    }
}