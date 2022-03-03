using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Dominio
{
    public class Cooperativo : Proyecto
    {
        [Required]
        public int CantidadIntegrantes { get; set; }
    }
}