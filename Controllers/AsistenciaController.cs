using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "1,5")]
        [HttpGet]
        [Route("GetAllAsistencias")]
        public async Task<IActionResult> GetAllAsistencias()
        {
            var resultado = await _asistenciaRepo.GetAllAsistencias();

            if (resultado.Count() == 0 || !resultado.Any())
            {
                return NotFound(new { message = "No se encontraron asistencia/s." });
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "1,4,5")]
        [HttpGet]
        [Route("GetAsistenciaById/{id}")]
        public async Task<IActionResult> GetAsistenciaById(int id)
        {
            var resultado = await _asistenciaRepo.GetAsistenciaUsersById(id);

            if (!resultado.Any() || resultado.Count() == 0) 
            {
                return Ok(new { message = "No se encontro la asistencia." });
            } 

            return Ok(resultado);
        }

        [Authorize(Roles = "1,4,5")]
        [HttpPost]
        [Route("AddAsistencia")]
        public async Task<IActionResult> AddAsistencia([FromBody] Asistencia asistencia)
        {
            if (asistencia == null) 
            {
                return BadRequest(new { message = "Asistencia no puede ser nulo" });
            }
                
            var resultado = await _asistenciaRepo.AddAsistencia(asistencia);

            if (resultado == null) 
            {
                return BadRequest(new { message = "Error al agregar asistencia" });
            } 

            return CreatedAtAction(nameof(GetAsistenciaById), new { id = resultado.Id }, resultado);
        }

        [Authorize(Roles = "1,5")]
        [HttpPut]
        [Route("UpdateAsistencia/{id}")]
        public async Task<IActionResult> UpdateAsistencia(int id, [FromBody] Asistencia asistencia)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null) 
            {
                return NotFound(new { message = "Asistencia no encontrada" });
            }

            if (asistencia == null) 
            {
                return BadRequest(new { message = "Asistencia no puede ser nulo" });
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

            if (resultado == null) 
            {
                return BadRequest(new { message = "Error al actualizar la asistencia" });
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "1,5")]
        [HttpPut]
        [Route("ChangeStateAsistencia/{id}")]
        public async Task<IActionResult> ChangeStateAsistencia(int id, [FromBody] bool activo)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null)
            {
                return NotFound(new { message = "Asistencia no encontrada" });
            }

            var resultado = await _asistenciaRepo.ChangeStateAsistencia(id, activo);

            if (!resultado)
            {
                return BadRequest(new { message = "Error al cambiar el estado de la asistencia" });
            }

            return Ok(new { message = "Asistencia actualizada correctamente" });
        }

        [HttpPut]
        [Route("UpdateAsistenciaSalida/{id}")]
        public async Task<IActionResult> UpdateAsistenciaSalida(int id)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null)
            {
                return NotFound(new { message = "Asistencia no encontrada" });
            }

            var resultado = await _asistenciaRepo.UpdateExitDate(id);

            if (!resultado)
            {
                return BadRequest(new { message = "Error al actualizar la asistencia" });
            }

            return Ok(resultado);
        }

        [Authorize(Roles = "1,5")]
        [HttpDelete]
        [Route("DeleteAsistencia/{id}")]
        public async Task<IActionResult> DeleteAsistencia(int id)
        {
            var asistenciaExistente = await _asistenciaRepo.GetAsistenciaById(id);

            if (asistenciaExistente == null)
            {
                return NotFound(new { message = "Asistencia no encontrada" });
            }

            var resultado = await _asistenciaRepo.DeleteAsistencia(id);

            if (!resultado)
            {
                return BadRequest(new { message = "Error al eliminar la asistencia" });
            }

            return Ok(resultado);
        }
    }
}
