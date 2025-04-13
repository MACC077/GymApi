namespace GymControlAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; } //Primary Key
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Contrasena { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Direccion { get; set; } = "";
        public DateTime FechaRegistro { get; set; } //Fecha de registro del usuario
        public bool Activo { get; set; } //Estado del usuario

        //Relacion con la tabla roles
        public int RolId { get; set; } //Foreign Key
        //public Rol Rol { get; set; } //Rol del usuario
    }
}
