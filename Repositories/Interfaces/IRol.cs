using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface IRol
    {
        Task<Rol> GetRolById(int id, bool incluirinactivos = false);
        Task<IEnumerable<Rol>> GetAllRoles(bool incluirinactivos = false);
        Task<Rol> AddRol(Rol rol);
        Task<Rol> UpdateRol(Rol rol);
        Task<bool> ChangeStateRol(int id, bool activo);
        Task<bool> DeleteRol(int id);
        Task<bool> SaveChangesAsync();

    }
}
