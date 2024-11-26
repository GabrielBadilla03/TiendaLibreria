using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Ventas
    {
        [Key]
        [Required]
        public int IdDetalle { get; set; }

        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public decimal Subtotal { get; set; }


        [ForeignKey("CodigoProducto")]
        public Producto Producto { get; set; }

        [Required]
        public int HistorialVentasId { get; set; } 

        [ForeignKey("HistorialVentasId")]
        public HistorialVenta HistorialVenta { get; set; }

    }
}