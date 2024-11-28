using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria.Models
{
    public class Producto
    {
        [Key]
        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreProducto { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public decimal Descuento { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int DisponibilidadInventario { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public ICollection<Reseña> Reseñas { get; set; }
       
        public byte[] ImagenProducto { get; set; }

        public ICollection<Reseña> Reseñas { get; set; }

    }
}