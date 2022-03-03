using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Validaciones;


namespace Dominio
{

    [Table("Usuarios")]
    public abstract class Usuario
    {

        public int Id { get; set; }
        //[Required]
        [MaxLength(10, ErrorMessage ="El nombre no puede tener más de 10 caracteres")]
        [Required]
        [Index(IsUnique = true)]
        [StringLength(8)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string Apellido { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha De Nacimiento")]
        [EdadUsuario(ErrorMessage = "Debe ser mayor de 21 para registrarse.")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [StringLength(9)]
        [Index(IsUnique = true)]
        [CelularValido(ErrorMessage = "El numero que ingresó no es válido.")]
        public string Celular { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "El correo ingresado no es válido.")]
//      [EmailUnico(ErrorMessage = "El correo que ingresó ya está registrado.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(30)]
        [PasswordValida(ErrorMessage = "La contraseña ingresada no cumple los requisitos.")]
        //  [PassValida(ErrorMessage = "Contraseña debe tener al menos 6 caracteres, 1 mayúscula, 1 minúscula y un numero.")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [StringLength(30)]
        public string ConfirmPassword { get; set; }

        public List<Proyecto> Proyectos { get; set; }
    }
}

