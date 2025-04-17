using GymControlAPI.Data;
using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class PagoRepo : IPago
    {
        private readonly GymDbContext _context;
        public PagoRepo(GymDbContext context)
        {
            _context = context;
        }
        public async Task<PagoDTO> GetPagoById(int id, bool incluirInactivos = false)
        {
            var resultado = await (
                from p in _context.Pagos
                join u in _context.Usuarios on p.UsuarioId equals u.Id
                join pl in _context.Planes on p.PlanId equals pl.Id
                join tp in _context.TipoPagos on p.TipoPagoId equals tp.Id
                where p.Activo || incluirInactivos
                && p.Id == id
                select new PagoDTO 
                { 
                    Id = p.Id,
                    FechaPago = p.FechaPago,
                    TipoPagoId = tp.Id,
                    TipoPagoNombre = tp.Nombre,
                    Activo = p.Activo,
                    FechaFin = p.FechaFin,
                    UsuarioId = u.Id,
                    UsuarioNombre = u.Nombre,
                    PlanId = pl.Id,
                    PlanNombre = pl.Nombre
                }

            ).FirstOrDefaultAsync();

            return resultado;
        }
        public async Task<IEnumerable<PagoDTO>> GetAllPago(bool incluirInactivos = false)
        {
            var resultado = await (
                from p in _context.Pagos 
                join u in _context.Usuarios on p.UsuarioId equals u.Id
                join pl in _context.Planes on p.PlanId equals pl.Id
                join tp in _context.TipoPagos on p.TipoPagoId equals tp.Id
                where p.Activo || incluirInactivos
                select new PagoDTO 
                {
                    Id = p.Id,
                    FechaPago = p.FechaPago,
                    TipoPagoId = tp.Id,
                    TipoPagoNombre = tp.Nombre,
                    Activo = p.Activo,
                    FechaFin = p.FechaFin,
                    UsuarioId = u.Id,
                    UsuarioNombre = u.Nombre,
                    PlanId = pl.Id,
                    PlanNombre = pl.Nombre
                }
            ).ToListAsync();

            return resultado;
        }
        public async Task<Pago> AddPago(Pago pago)
        {
            await _context.Pagos.AddAsync(pago);
            await SaveChanges();
            return pago;
        }
        public async Task<Pago> UpdatePago(Pago pago)
        {
            _context.Pagos.Update(pago);
            await SaveChanges();
            return pago;
        }

        public async Task<bool> ChangeStatePago(int id, bool activo)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null) return false;
            pago.Activo = activo;
            _context.Pagos.Update(pago);
            return await SaveChanges();
        }

        public async Task<bool> DeletePago(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            _context.Pagos.Remove(pago);
            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
