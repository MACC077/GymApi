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

    }
}
