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
        public int IdDetalle { get; set; } // Identificador único del detalle de la venta

        [Required]
        public int CodigoProducto { get; set; } // Código del producto

        [ForeignKey("CodigoProducto")]
        public Producto Producto { get; set; } // Relación con la tabla Producto

        [Required]
        public int Cantidad { get; set; } // Cantidad de productos vendidos

        [Required]
        public decimal PrecioUnitario { get; set; } // Precio unitario del producto

        [NotMapped]
        public decimal PrecioTotal => Cantidad * PrecioUnitario; // Calculado en tiempo de ejecución


        [Required]
        public int HistorialVentasId { get; set; } // Relación con HistorialVenta

        [ForeignKey("HistorialVentasId")]
        public HistorialVenta HistorialVenta { get; set; } // Relación con la tabla HistorialVenta
    }
}