using GymControlAPI.DTOs;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;

        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpPost]
        [Route("LoginAuth")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login) {

            if (login == null)
            {
                return BadRequest(new { message = "Los datos de inicio de sesión no pueden ser nulos." });
            }

            if (string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Contrasena))
            {
                return BadRequest(new { message = "El usuario y la contraseña son obligatorios." });
            }

            var usuario = await _login.AuthenticateUser(login);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
            }

            string token = await _login.GenerateToken(usuario);

            return Ok(new { token });
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirst("id")?.Value; // Capturamos el Id del token usando el Claim
            
            if (userId == null)
            {
                return Unauthorized(new { message = "Token sin informacion del usuario" });
            }

            var usuarioData = await _login.GetCurrentUser(int.Parse(userId));

            if (usuarioData == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            return Ok(usuarioData);
        }

    }
}
