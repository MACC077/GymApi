using GymControlAPI.Data;
using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class UsuarioRepo : IUsuario
    {
        private readonly GymDbContext _context;
        public UsuarioRepo(GymDbContext context)
        {
            _context = context;
        }
        public async Task<UsuarioDTO> GetUsuarioById(int id, bool incluirinactivos = false)
        {
            var resultado = await(
                from u in _context.Usuarios
                join r in _context.Roles on u.RolId equals r.Id
                where u.Activo || incluirinactivos //Si el usuario está activo o si se permite incluir inactivos
                && u.Id == id
                select new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono,
                    Correo = u.Correo,
                    Direccion = u.Direccion,
                    FechaRegistro = u.FechaRegistro,
                    Activo = u.Activo,
                    RolId = u.RolId,
                    RolNombre = r.Nombre
                }
            ).FirstOrDefaultAsync();

            return resultado;
            //await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuarios(bool incluirinactivos = false)
        {
            var resultado = await (
                from u in _context.Usuarios
                join r in _context.Roles on u.RolId equals r.Id
                where u.Activo || incluirinactivos //Si el usuario está activo o si se permite incluir inactivos
                select new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono,
                    Correo = u.Correo,
                    Direccion = u.Direccion,
                    FechaRegistro = u.FechaRegistro,
                    Activo = u.Activo,
                    RolId = u.RolId,
                    RolNombre = r.Nombre
                }
            ).ToListAsync();
            
            return resultado;
        }
        //From here to down it's work with the original model
        public async Task<Usuario> AddUsuario(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await SaveChangesAsync();
            return usuario;
        }
        public async Task<Usuario> UpdateUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await SaveChangesAsync();
            return usuario;
        }
        //Note: for the methods where the DTOs are not used, we can use the original model
        public async Task<bool> ChangeStateUsuario(int id, bool activo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return false;
            usuario.Activo = activo;
            _context.Usuarios.Update(usuario);
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id && u.Activo);
            if (usuario == null) return false;
            _context.Usuarios.Remove(usuario);
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
