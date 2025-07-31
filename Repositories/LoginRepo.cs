using GymControlAPI.Data;
using GymControlAPI.DTOs;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymControlAPI.Repositories
{
    public class LoginRepo : ILogin
    {
        private readonly GymDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginRepo(GymDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Usuario> AuthenticateUser(LoginDTO login)
        {
            var usuario = await _context.Usuarios
                                .FirstOrDefaultAsync(u =>
                                u.Correo == login.Usuario
                                && u.Contrasena == login.Contrasena
                                && u.Activo);
            return usuario;
        }

        public async Task<string> GenerateToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expire = jwtSettings["ExpirationInMinutes"];

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim("id", usuario.Id.ToString()),
                   new(ClaimTypes.Name, usuario.Correo),
                   new(ClaimTypes.Role, usuario.RolId.ToString()), // Convert RolId (int) to string  
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(JwtRegisteredClaimNames.Iat,
                   DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                   ClaimValueTypes.Integer64)
                }),

                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expire)),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                                     new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UsuarioDTO> GetCurrentUser(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id && u.Activo)
                .Join(_context.Planes,
                    u => u.PlanId,
                    p => p.Id,
                    (u, p) => new UsuarioDTO
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Apellido = u.Apellido,
                        Correo = u.Correo,
                        RolId = u.RolId,
                        Telefono = u.Telefono,
                        Direccion = u.Direccion,
                        Plan = p.Nombre,
                        PlanId = Convert.ToInt32(p.Id)
                    }
                )
                .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync to return a single UsuarioDTO

            return usuario;
        }
    }
}
