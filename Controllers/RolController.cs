using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _rolRepo.GetAllRoles();

            if (roles == null || !roles.Any())
            {
                return NotFound(new { message = "No se encontraron roles." });
            }

            return Ok(roles);
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetRolById/{id}")]
        public async Task<IActionResult> GetRolById(int id)
        {
            var rol = await _rolRepo.GetRolById(id);

            if (rol == null)
            {
                return NotFound(new { message = "Rol no encontrado." });
            }

            return Ok(rol);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        [Route("AddRol")]
        public async Task<IActionResult> AddRol([FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest(new { message = "El rol no puede ser nulo." });
            }

            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest(new { message = "El nombre del rol es obligatorio." });
            }
            
            rol.Activo = true;
            rol.FechaRegistro = DateTime.Now;

            var nuevoRol = await _rolRepo.AddRol(rol);

            return CreatedAtAction(nameof(GetRolById), new { id = nuevoRol.Id }, nuevoRol);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("UpdateRol/{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] Rol rol)
        {
            if (rol == null)
            {
                return BadRequest(new { message = "El rol no puede ser nulo." });
            }

            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest(new { message = "El nombre del rol es obligatorio." });
            }

            var rolExistente = await _rolRepo.GetRolById(id);

            if (rolExistente == null)
            {
                return NotFound(new { message = "Rol no encontrado." });
            }

            rolExistente.Nombre = rol.Nombre;
            rolExistente.Activo = rol.Activo;

            var rolActualizado = await _rolRepo.UpdateRol(rolExistente);
            return Ok(rolActualizado);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("UpdateRolStatus/{id}")]
        public async Task<IActionResult> UpdateRolStatus(int id, [FromBody] bool activo)
        {
            var resultado = await _rolRepo.ChangeStateRol(id, activo);

            if (!resultado)
            {
                return BadRequest(new { message = "Error al cambiar el estado del rol." });
            }

            return Ok(new { message = "Estado actualizado correctamente." });
        }

        [Authorize(Roles = "1")]
        [HttpDelete]
        [Route("DeleteRol/{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _rolRepo.GetRolById(id);

            if (rol == null)
            {
                return NotFound(new { message = "Rol no encontrado." });
            }

            var resultado = await _rolRepo.DeleteRol(id);

            if (!resultado)
            {
                return BadRequest(new { message = "Error al eliminar el rol." });
            }

            return NoContent();
        }
    }
}
