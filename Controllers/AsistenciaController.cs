using GymControlAPI.DTOs;
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
        public async Task<ActionResult<IEnumerable<AsistenciaDTO>>> GetAllAsistencias(bool incluirInactivos = false)
        {
            var resultado = await _asistenciaRepo.GetAllAsistencias(incluirInactivos);
            return Ok(resultado);
        }

        [HttpGet]
        [Route("GetAsistenciaById/{id}")]
        public async Task<ActionResult<AsistenciaDTO>> GetAsistenciaById(int id)
        {
            var resultado = await _asistenciaRepo.GetAsistenciaById(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }
    }
}
