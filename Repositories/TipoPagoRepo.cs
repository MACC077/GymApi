using GymControlAPI.Data;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class TipoPagoRepo : ITipoPago
    {
        private readonly GymDbContext _context;


        public TipoPagoRepo(GymDbContext context)
        {
            _context = context;
        }

        public async Task<TipoPago> GetTipoPagoById(int id, bool incluirinactivos = false)
        {
            var resultado = await _context.TipoPagos
                .Where(tp => tp.Id == id && (tp.Activo || incluirinactivos))
                .FirstOrDefaultAsync();
            return resultado;
        }

        public async Task<IEnumerable<TipoPago>> GetAllTipoPagos(bool incluirinactivos = false)
        {
            var resultado = await _context.TipoPagos
                .Where(tp => tp.Activo || incluirinactivos)
                .ToListAsync();
            return resultado;
        }

        public async Task<TipoPago> AddTipoPago(TipoPago tipoPago)
        {
            await _context.TipoPagos.AddAsync(tipoPago);
            await SaveChangesAsync();
            return tipoPago;
        }

        public async Task<TipoPago> UpdateTipoPago(TipoPago tipoPago)
        {
            _context.TipoPagos.Update(tipoPago);
            await SaveChangesAsync();
            return tipoPago;
        }

        public async Task<bool> ChangeStateTipoPago(int id, bool activo)
        {
            var tipoPago = await GetTipoPagoById(id, true);
            if (tipoPago == null) return false;
            tipoPago.Activo = activo;
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTipoPago(int id)
        {
            var tipoPago = await GetTipoPagoById(id);
            if (tipoPago == null) return false;
            tipoPago.Activo = false;
            _context.TipoPagos.Update(tipoPago);
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }   
    }
}
