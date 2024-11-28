using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(100)] 
        public string NombreCategoria { get; set; }

        [MaxLength(500)]
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}