
using System.ComponentModel.DataAnnotations;
namespace ProyectoIntuit.Models

{
    public class Cliente
    {//Nombres, apellidos, CUIT, teléfono celular, email -> que sean obligatorios

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string RazonSocial { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "El cuit debe conetener 13 caracteres (incluir -)")] 
        public string Cuit { get; set; } = null!;

        //public string Domicilio { get; set;}

        public string Telefono { get; set; } = null!;


        [EmailAddress(ErrorMessage = "El formato del Email no es correcto")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaModificacion { get; set; }


    }
    
}
