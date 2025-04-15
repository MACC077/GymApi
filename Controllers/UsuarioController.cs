using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
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

        [HttpGet]
        [Route("GetAllUsuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioRepo.GetAllUsuarios();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("GetUsuarioById/{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepo.GetUsuarioById(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("AddUsuario")]
        public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("El usuario no puede ser nulo.");
            }

            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Apellido))
            {
                return BadRequest("El nombre y apellido son obligatorios.");
            }

            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Contrasena))
            {
                return BadRequest("El correo y la contraseña son obligatorios.");
            }

            if (string.IsNullOrEmpty(usuario.Telefono) || string.IsNullOrEmpty(usuario.Direccion))
            {
                return BadRequest("El teléfono y la dirección son obligatorios.");
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
                return BadRequest("El usuario no puede ser nulo.");
            }

            //Validamos que el usuario exista
            var usuarioExistente = await _usuarioRepo.GetUsuarioById(id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var usuarioUpdate = new Usuario
            {
                Id = id,//Este es el Id que viene por parametro
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                FechaRegistro = usuarioExistente.FechaRegistro,
                RolId = usuario.RolId,
                Activo = usuario.Activo
            };

            var resultado = await _usuarioRepo.UpdateUsuario(usuarioUpdate);
            return Ok(resultado);
        }

        [HttpPut]
        [Route("ChangeStateUsuario/{id}")]
        public async Task<IActionResult> ChangeStateUsuario(int id, [FromBody] bool activo)
        {
            var actualizado = await _usuarioRepo.ChangeStateUsuario(id, activo);

            if (!actualizado)
            {
                return NotFound("Usuario no encontrado.");
            }
               
            return Ok("Estado actualizado correctamente.");
        }

        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuarioRepo.GetUsuarioById(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            var resultado = await _usuarioRepo.DeleteUsuario(id);

            if (!resultado)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }

            return NoContent();
        }
    }
}
