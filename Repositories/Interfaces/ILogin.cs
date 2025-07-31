using GymControlAPI.DTOs;
using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface ILogin
    {
        public Task<Usuario> AuthenticateUser(LoginDTO login);
        public Task<string> GenerateToken(Usuario usuario);
        public Task<UsuarioDTO> GetCurrentUser(int id);
    }
}
