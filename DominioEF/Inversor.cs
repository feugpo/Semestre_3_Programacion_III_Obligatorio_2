using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio
{
    public class Inversor : Usuario
    {
        [Required]
        public decimal MontoMaximo { get; set; }

        [Required]
        [StringLength(500)]
        public string Presentacion { get; set; }

        public List<Financiamiento> Financiamientos { get; set; }
    }
}