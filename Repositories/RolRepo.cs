using GymControlAPI.Data;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class RolRepo : IRol
    {
        private readonly GymDbContext _context;
        public RolRepo(GymDbContext context)
        {
            _context = context;
        }
        //Implementar métodos de la interfaz IRol

        public async Task<Rol> GetRolById(int id, bool incluirinactivos = false)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && (r.Activo || incluirinactivos));
        }

        public async Task<IEnumerable<Rol>> GetAllRoles(bool incluirinactivos = false)
        {
            return await _context.Roles.Where(r => r.Activo|| incluirinactivos).ToListAsync();
        }

        public async Task<Rol> AddRol(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
            await SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> UpdateRol(Rol rol)
        { 
            _context.Roles.Update(rol);
            await SaveChangesAsync();
            return rol;
        }

        public async Task<bool> ChangeStateRol(int id, bool activo)
        {
            var rol = await GetRolById(id, true);

            if (rol == null)
            {
                return false;
            }
            rol.Activo = activo;
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRol(int id)
        {
            var rol = await GetRolById(id);
            if (rol == null)
            {
                return false;
            }
            _context.Roles.Remove(rol);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
