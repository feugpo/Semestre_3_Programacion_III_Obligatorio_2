using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio
{
    [Table("Financiamientos")]
    public class Financiamiento

    {
        public int Id { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public DateTime FechaFinanciacion { get; set; }


        [ForeignKey("FKProyecto")]
        public Proyecto Proyecto { get; set; }

        public int FKProyecto { get; set; }
       

        [ForeignKey("FKInversor")]
        public Inversor Inversor { get; set; }

        public int FKInversor { get; set; }
    }
}