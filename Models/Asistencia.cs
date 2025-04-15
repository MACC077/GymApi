namespace GymControlAPI.Models
{
    public class Asistencia
    {
        public int Id { get; set; } //Primary Key Id
        public DateTime Fecha { get; set; } //Fecha de la asistencia
        public TimeSpan HoraEntrada { get; set; } //Hora de entrada
        public TimeSpan? HoraSalida { get; set; } //Hora de salida
        public bool Activo { get; set; } = true; //Estado de la asistencia (Activo/Inactivo)
        public DateTime FechaRegistro { get; set; } = DateTime.Now; //Fecha de registro de la asistencia
        //Relacion con la tabla usuarios
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
