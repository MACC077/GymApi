using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface ITipoPago
    {
        public Task<TipoPago> GetTipoPagoById(int id, bool incluirinactivos = false);
        public Task<IEnumerable<TipoPago>> GetAllTipoPagos(bool incluirinactivos = false);
        public Task<TipoPago> AddTipoPago(TipoPago tipoPago);
        public Task<TipoPago> UpdateTipoPago(TipoPago tipoPago);

        public Task<bool> ChangeStateTipoPago(int id, bool activo);
        public Task<bool> DeleteTipoPago(int id);
        public Task<bool> SaveChangesAsync();
    }
}
