using System.Text.Json.Serialization;

namespace GymControlAPI.Models
{
    public class Pago
    {
        public int Id { get; set; } //Primary Key Id
        public decimal Monto { get; set; } //Monto del pago
        public DateTime FechaPago { get; set; } //Fecha de pago
        public int TipoPagoId { get; set; } //Metodo de pago (Efectivo, Tarjeta, Transferencia, Etc..)
        public bool Activo { get; set; } = true; //Estado del pago (Activo/Inactivo)
        public DateTime FechaFin { get; set; } = DateTime.Now; //Fecha de fin del pago
        //Relacion con la tabla usuarios
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        //Relacion con la tabla planes
        public int PlanId { get; set; }
        //[JsonIgnore]
        //public Plan? Plan { get; set; } = null!;
    }
}
