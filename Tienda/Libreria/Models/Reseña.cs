using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Reseña
    {
        [Key]
        [Required]
        public int ReseñaId { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public string CodigoCliente { get; set; } // Cambiado a string para coincidir con AspNetUsers.Id

        [Required]
        [MaxLength(1000)]
        public string Comentario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }

        [Required]
        public DateTime FechaReseña { get; set; } = DateTime.Now;

        [ForeignKey("CodigoProducto")]
        public Producto Producto { get; set; }

        [ForeignKey("CodigoCliente")]
        public ApplicationUser Usuario { get; set; } // Relación con ApplicationUser
    }
}