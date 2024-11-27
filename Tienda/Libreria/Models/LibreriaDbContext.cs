using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Libreria.Models
{
    public class LibreriaDbContext : DbContext
    {
        public LibreriaDbContext() : base("DefaultConnection") {}

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<HistorialVenta> HistorialVentas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Ventas> Ventas { get; set; }
    }
}