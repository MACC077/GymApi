using GymControlAPI.Data;
using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class AsistenciaRepo: IAsistencia
    {
        private readonly GymDbContext _context;

        public AsistenciaRepo(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AsistenciaDTO>> GetAllAsistencias(bool incluirInactivos = false)
        {
            var asistencias = await (
               from a in _context.Asistencias
               join u in _context.Usuarios on a.UsuarioId equals u.Id
               where a.Activo || incluirInactivos
               select new AsistenciaDTO
               {
                   Id = a.Id,
                   HoraEntrada = a.HoraEntrada,
                   HoraSalida = a.HoraSalida,
                   Activo = a.Activo,
                   FechaRegistro = a.FechaRegistro ?? default(DateTime), // Handle nullable value
                   UsuarioId = u.Id,
                   UsuarioNombre = u.Nombre
               }
            ).ToListAsync();

            return asistencias;
        }
        public async Task<AsistenciaDTO> GetAsistenciaById(int id, bool incluirInactivos = false)
        {
            var asistencia = await (
                from a in _context.Asistencias
                join u in _context.Usuarios on a.UsuarioId equals u.Id
                where a.Activo || incluirInactivos
                && a.Id == id
                select new AsistenciaDTO
                {
                    Id = a.Id,
                    HoraEntrada = a.HoraEntrada,
                    HoraSalida = a.HoraSalida,
                    Activo = a.Activo,
                    FechaRegistro = a.FechaRegistro ?? default(DateTime), // Handle nullable value
                    UsuarioId = u.Id,
                    UsuarioNombre = u.Nombre
                }
            ).FirstOrDefaultAsync();

            return asistencia;
        }
        public async Task<Asistencia> AddAsistencia(Asistencia asistencia)
        {
            await _context.Asistencias.AddAsync(asistencia); // Use AddAsync instead of Add
            await SaveChanges(); // Ensure SaveChanges is awaited
            return asistencia;
        }
        public async Task<Asistencia> UpdateAsistencia(Asistencia asistencia)
        {
            _context.Asistencias.Update(asistencia);
            await SaveChanges();
            return asistencia;
        }
        public async Task<bool> ChangeStateAsistencia(int id, bool activo)
        {
            var asistencia = await _context.Asistencias.FirstOrDefaultAsync(a => a.Id == id);
            if (asistencia == null)
            {
                return false;
            }
            asistencia.Activo = activo;
            return await SaveChanges();
        }

        public async Task<bool> UpdateExitDate(int id)
        {
            var asistenciaExistente = await _context.Asistencias.FirstOrDefaultAsync(a => a.Id == id);

            if (asistenciaExistente == null)
            {
                return false;
            }
            asistenciaExistente.HoraSalida = DateTime.Now;
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsistencia(int id)
        {
            var asistencia = await _context.Asistencias.FirstOrDefaultAsync(a => a.Id == id);

            if (asistencia == null)
            {
                return false;
            }
            _context.Asistencias.Remove(asistencia);
            return await SaveChanges();
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
