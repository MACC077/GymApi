using GymControlAPI.DTOs;
using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface IUsuario
    {
        //Devuelvo data para visualizar con el DTO
        Task<UsuarioDTO> GetUsuarioById(int id, bool incluirinactivos = false);
        Task<IEnumerable<UsuarioDTO>> GetAllUsuarios(bool incluirinactivos = false);
        //Devuelvo el objeto completo para agregar o modificar
        Task<Usuario> AddUsuario(Usuario usuario);
        Task<Usuario> UpdateUsuario(Usuario usuario);
        Task<bool> ChangeStateUsuario(int id, bool activo);
        Task<bool> DeleteUsuario(int id);
        Task<bool> SaveChangesAsync();
    }
}
