namespace GymControlAPI.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } 
        public bool Activo { get; set; } 
        public int RolId { get; set; }
        public string RolNombre { get; set; } //Nombre del rol
        public int PlanId { get; set; } //Id del plan asociado al usuario
        public string Plan { get; set; } //Nombre del plan asociado al usuario
    }
}
