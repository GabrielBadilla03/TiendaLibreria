using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class HistorialVenta
    {
        [Key]
        [Required]
        public int HistorialVentasId { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public int CantidadProductosVendidos { get; set; } 

        [Required]
        public decimal GananciaTotal { get; set; }

        public ICollection<Ventas> Ventas { get; set; }

    }
}