using GymControlAPI.DTOs;
using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface IAsistencia
    {
        public Task<AsistenciaDTO> GetAsistenciaById(int id, bool incluirInactivos = false);
        public Task<IEnumerable<AsistenciaDTO>> GetAllAsistencias(bool incluirInactivos = false);
        public Task<Asistencia> AddAsistencia(Asistencia asistencia);
        public Task<Asistencia> UpdateAsistencia(Asistencia asistencia);
        public Task<bool> ChangeStateAsistencia(int id, bool activo);
        public Task<bool> UpdateExitDate(int id);
        public Task<bool> DeleteAsistencia(int id);
        public Task<bool> SaveChanges();
    }
}
