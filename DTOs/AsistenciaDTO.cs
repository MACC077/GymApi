namespace GymControlAPI.DTOs
{
    public class AsistenciaDTO
    {
        public int? Id { get; set; } //Primary Key Id
        public DateTime? HoraEntrada { get; set; } = DateTime.Now; //Hora de entrada
        public DateTime? HoraSalida { get; set; } //Hora de salida
        public bool Activo { get; set; } = true; //Estado de la asistencia (Activo/Inactivo)
        public DateTime FechaRegistro { get; set; } = DateTime.Now; //Fecha de registro de la asistencia
        //Relacion con la tabla usuarios
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = null!;
    }
}
