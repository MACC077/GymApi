namespace GymControlAPI.Models
{
    public class Rol
    {
        public int? Id { get; set; } //Primary Key Id
        public string Nombre { get; set; } //Nombre del rol (Admin,Cliente,Coach,Etc..)
        public bool Activo { get; set; } //Estado del rol (Activo/Inactivo)
        public DateTime FechaRegistro { get; set; } //Fecha de registro del rol

        //Relacion con la tabla usuarios
        //public ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();
    }
}
