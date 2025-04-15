namespace GymControlAPI.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionDias { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        // Relación con la tabla de usuarios
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
