namespace GymControlAPI.Models
{
    public class TipoPago
    {
        public int? Id { get; set; } //Primary Key Id
        public string Nombre { get; set; } //Nombre del tipo de pago (Efectivo, Tarjeta, Transferencia, Etc..)
        public bool Activo { get; set; } = true; //Estado del tipo de pago (Activo/Inactivo)
        public DateTime? FechaRegistro { get; set; } = DateTime.Now; //Fecha de registro del tipo de pago
    }
}
