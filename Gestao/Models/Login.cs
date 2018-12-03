using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class Login
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string senha { get; set; }

    }
}