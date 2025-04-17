using GymControlAPI.DTOs;
using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface IPago
    {
        public Task<PagoDTO> GetPagoById(int id, bool incluirInactivos = false);
        public Task<IEnumerable<PagoDTO>> GetAllPago(bool incluirInactivos = false);
        public Task<Pago> AddPago(Pago pago);
        public Task<Pago> UpdatePago(Pago pago);
        public Task<bool> ChangeStatePago(int id, bool activo);
        public Task<bool> DeletePago(int id);
        public Task<bool> SaveChanges();
    }
}
