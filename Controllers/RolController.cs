using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRol _rolRepo;

        public RolController(IRol rolRepo)
        {
            _rolRepo = rolRepo;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _rolRepo.GetAllRoles();

            if (roles == null || !roles.Any())
            {
                return NotFound("No se encontraron roles.");
            }

            return Ok(roles);
        }

        [HttpGet]
        [Route("GetRolById/{id}")]
        public async Task<IActionResult> GetRolById(int id)
        {
            var rol = await _rolRepo.GetRolById(id);

            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return Ok(rol);
        }

        [HttpPost]
        [Route("AddRol")]
        public async Task<IActionResult> AddRol([FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest("El rol no puede ser nulo.");
            }
            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest("El nombre del rol es obligatorio.");
            }
            var nuevoRol = await _rolRepo.AddRol(rol);
            return CreatedAtAction(nameof(GetRolById), new { id = nuevoRol.Id }, nuevoRol);
        }

        [HttpPut]
        [Route("UpdateRol/{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest("El rol no puede ser nulo.");
            }

            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest("El nombre del rol es obligatorio.");
            }

            var rolExistente = await _rolRepo.GetRolById(id);

            if (rolExistente == null)
            {
                return NotFound("Rol no encontrado.");
            }

            rolExistente.Nombre = rol.Nombre;
            rolExistente.Activo = rol.Activo;

            var rolActualizado = await _rolRepo.UpdateRol(rolExistente);
            return Ok(rolActualizado);
        }

        [HttpPut]
        [Route("UpdateRolStatus/{id}")]
        public async Task<IActionResult> UpdateRolStatus(int id, [FromBody] bool activo)
        {
            var resultado = await _rolRepo.ChangeStateRol(id, activo);

            if (!resultado)
            {
                return BadRequest("Error al cambiar el estado del rol.");
            }

            return Ok("Estado actualizado correctamente.");
        }

        [HttpDelete]
        [Route("DeleteRol/{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _rolRepo.GetRolById(id);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }
            var resultado = await _rolRepo.DeleteRol(id);
            if (!resultado)
            {
                return BadRequest("Error al eliminar el rol.");
            }
            return NoContent();
        }
    }
}
