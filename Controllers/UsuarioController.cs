using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuarioRepo;
        public UsuarioController(IUsuario usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetAllUsuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioRepo.GetAllUsuarios();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound(new { message = "No se encontraron usuarios." });
            }

            return Ok(usuarios);
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetUsuarioById/{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepo.GetUsuarioByIdDTO(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("AddUsuario")]
        public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { message = "El usuario no puede ser nulo." });
            }

            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Apellido))
            {
                return BadRequest(new { message = "El nombre y apellido son obligatorios." });
            }

            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Contrasena))
            {
                return BadRequest(new { message = "El correo y la contraseña son obligatorios." });
            }

            if (string.IsNullOrEmpty(usuario.Telefono) || string.IsNullOrEmpty(usuario.Direccion))
            {
                return BadRequest(new { message = "El teléfono y la dirección son obligatorios." });
            }

            var usuarioNuevo = new Usuario
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Contrasena = usuario.Contrasena,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                RolId = usuario.RolId,
                PlanId = usuario.PlanId,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            var nuevoUsuario = await _usuarioRepo.AddUsuario(usuarioNuevo);
            return CreatedAtAction(nameof(GetUsuarioById), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [HttpPut]
        [Route("UpdateUsuario/{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { message = "El usuario no puede ser nulo." });
            }

            //Validamos que el usuario exista
            var usuarioExistente = await _usuarioRepo.GetUsuarioById(id);

            if (usuarioExistente == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Telefono = usuario.Telefono;
            usuarioExistente.Direccion = usuario.Direccion;
            usuarioExistente.RolId = usuario.RolId;
            usuarioExistente.PlanId = usuario.PlanId;

            var resultado = await _usuarioRepo.UpdateUsuario(usuarioExistente);
            return Ok(resultado);
        }

        [Authorize(Roles = "1,5")]
        [HttpPut]
        [Route("ChangeStateUsuario/{id}")]
        public async Task<IActionResult> ChangeStateUsuario(int id, [FromBody] bool activo)
        {
            var actualizado = await _usuarioRepo.ChangeStateUsuario(id, activo);

            if (!actualizado)
            {
                return NotFound(new { message =  "Usuario no encontrado." });
            }
               
            return Ok(new { message =  "Estado actualizado correctamente." });
        }

        [Authorize(Roles = "1")]
        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuarioRepo.GetUsuarioById(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            var resultado = await _usuarioRepo.DeleteUsuario(id);

            if (!resultado)
            {
                return BadRequest(new { message = "No se pudo eliminar el usuario." });
            }

            return NoContent();
        }
    }
}
