using Microsoft.EntityFrameworkCore;
using Papeleria.Domain.Entities;

namespace Papeleria.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 👤 Usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        // 📦 Productos y cursos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // 🛒 Carrito
        public DbSet<CarroDeCompras> Carros { get; set; }
        public DbSet<CarroItem> CarroItems { get; set; }

        // 🧾 Facturación
        public DbSet<FacturaHeader> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔗 Relaciones
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Categoria)
                .WithMany()
                .HasForeignKey(c => c.CategoriaId);

            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Producto)
                .WithMany()
                .HasForeignKey(fd => fd.ProductoId);

            modelBuilder.Entity<FacturaHeader>()
                .HasMany(f => f.Detalles)
                .WithOne()
                .HasForeignKey(fd => fd.FacturaId);

            // 💰 Configuración de precisión decimal
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Curso>()
                .Property(c => c.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.PrecioUnitario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FacturaHeader>()
                .Property(f => f.Total)
                .HasPrecision(10, 2);
            modelBuilder.Entity<CarroItem>()
            .HasOne(ci => ci.Producto)
            .WithMany()
            .HasForeignKey(ci => ci.ProductoId);
        }
    }
}
