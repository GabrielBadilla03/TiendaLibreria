using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Carrito
    {
        [Key]
        public int CarritoId { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [ForeignKey("CodigoProducto")]
        public Producto Producto { get; set; }

        [Required]
        public int? Cantidad { get; set; } 

        public decimal Subtotal => (Cantidad ?? 0) * Producto.Precio; 
    }
}
