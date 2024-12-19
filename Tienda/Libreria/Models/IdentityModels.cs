using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Libreria.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que authenticationType debe coincidir con el valor definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar reclamaciones de usuario personalizadas aquí
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Ventas> Ventas { get; set; }
        public DbSet<HistorialVenta> HistorialVentas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Producto>()
                .HasRequired(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Reseña>()
                .HasRequired(r => r.Producto)
                .WithMany(p => p.Reseñas)
                .HasForeignKey(r => r.CodigoProducto);

            // Configuración de la relación entre Reseña y ApplicationUser
            modelBuilder.Entity<Reseña>()
                .HasRequired(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.CodigoCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ventas>()
    .HasRequired(v => v.Producto)
    .WithMany()
    .HasForeignKey(v => v.CodigoProducto);

            modelBuilder.Entity<Ventas>()
                .HasRequired(v => v.HistorialVenta)
                .WithMany(h => h.Ventas)
                .HasForeignKey(v => v.HistorialVentasId);


            base.OnModelCreating(modelBuilder);
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}