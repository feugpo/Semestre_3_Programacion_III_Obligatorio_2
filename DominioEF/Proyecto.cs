using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;


namespace Dominio
{
    [Table ("Proyectos")]
    public abstract class Proyecto
    {
        public int Id { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
    //    [TituloUnico(ErrorMessage = "El nombre del proyecto ingresado ya existe.")]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(150)]
        public string Descripcion { get; set; }
        [Required]
        public decimal MontoTotal { get; set; }
        
        public decimal MontoTotalConInteres { get; set; }

        public decimal MontoCuotas { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Ingrese un numero positivo.")]
     // [RangoCuotas(ErrorMessage = "El valor de cuotas ingresado está fuera de rango.")]
        public int Cuotas { get; set; }

       
        public string Imagen { get; set; } //COMO ES EL FORMATO?

        [Column("Interes")]
        public decimal TasaInteres { get; set; }


        public string Estado { get; set; }
        

        public DateTime FechaCreacion { get; set; }
    }
}

