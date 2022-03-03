using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio

{
    public class Personal : Proyecto
    {
        [Required]
        [StringLength(200)]
        public string Experiencia { get; set; }
    }

}