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
    }
}
