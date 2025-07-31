using GymControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) 
        { 
        
        }
        
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Plan> Planes { get; set; } = null!;
        public DbSet<Pago> Pagos { get; set; } = null!;
        public DbSet<Asistencia> Asistencias { get; set; } = null!;
        public DbSet<TipoPago> TipoPagos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuración de las entidades
            // Agregar FKs
            modelBuilder.Entity<Usuario>()
                .HasOne<Rol>()
                .WithMany()
                .HasForeignKey(fk => fk.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Usuario>()
            //    .HasOne<Plan>()
            //    .WithMany()
            //    .HasForeignKey(fk => fk.PlanId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asistencia>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
