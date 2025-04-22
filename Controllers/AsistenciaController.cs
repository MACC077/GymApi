using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistencia _asistenciaRepo;
        public AsistenciaController(IAsistencia asistenciaRepo)
        {
            _asistenciaRepo = asistenciaRepo;
        }

        [HttpGet]
        [Route("GetAllAsistencias")]
        public async Task<IActionResult> GetAllAsistencias()
        {
            var resultado = await _asistenciaRepo.GetAllAsistencias();
            return Ok(resultado);
        }

        [HttpGet]
        [Route("GetAsistenciaById/{id}")]
        public async Task<IActionResult> GetAsistenciaById(int id)
        {
            var resultado = await _asistenciaRepo.GetAsistenciaById(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }

        [HttpPost]
        [Route("AddAsistencia")]
        public async Task<IActionResult> AddAsistencia([FromBody] Asistencia asistencia)
        {
            if (asistencia == null) return BadRequest("Asistencia no puede ser nulo");
            var resultado = await _asistenciaRepo.AddAsistencia(asistencia);
            if (resultado == null) return BadRequest("Error al agregar asistencia");
            return CreatedAtAction(nameof(GetAsistenciaById), new { id = resultado.Id }, resultado);
        }

        [HttpPut]
        [Route("UpdateAsistencia/{id}")]
        public async Task<IActionResult> UpdateAsistencia(int id, [FromBody] Asistencia asistencia)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null) 
            {
                return NotFound("Asistencia no encontrada");
            }

            if (asistencia == null) 
            {
                return BadRequest("Asistencia no puede ser nulo");
            }

            var asistenciaActualizada = new Asistencia
            {
                Id = id,
                HoraEntrada = asistencia.HoraEntrada,
                HoraSalida = asistencia.HoraSalida,
                Activo = asistencia.Activo,
                FechaRegistro = asistenciaExistente.FechaRegistro,
                UsuarioId = asistencia.UsuarioId
            };

            var resultado = await _asistenciaRepo.UpdateAsistencia(asistenciaActualizada);
            return Ok(resultado);
        }

        [HttpPut]
        [Route("ChangeStateAsistencia/{id}")]
        public async Task<IActionResult> ChangeStateAsistencia(int id, [FromBody] bool activo)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null)
            {
                return NotFound("Asistencia no encontrada");
            }

            var resultado = await _asistenciaRepo.ChangeStateAsistencia(id, activo);
            return Ok(resultado);
        }

        [HttpDelete]
        [Route("DeleteAsistencia/{id}")]
        public async Task<IActionResult> DeleteAsistencia(int id)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null)
            {
                return NotFound("Asistencia no encontrada");
            }

            var resultado = await _asistenciaRepo.DeleteAsistencia(id);
            return Ok(resultado);
        }
    }
}
