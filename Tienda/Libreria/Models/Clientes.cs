using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Clientes
    {
        [Key]
        [Required]
        public int ClienteId { get; set; } 

        [Required]
        [MaxLength(100)] 
        public string NombreCompleto { get; set; }

        [Required]
        [MaxLength(100)] 
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [MaxLength(20)] 
        [Phone] 
        public string Telefono { get; set; }

        [Required]
        [MaxLength(200)] 
        public string Direccion { get; set; }

        [Required]
        public string Contrasena { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now; 

        public ICollection<Reseña> Reseñas { get; set; }
    }
}