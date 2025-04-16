using System.Text.Json.Serialization;

namespace GymControlAPI.Models
{
    public class Pago
    {
        public int? Id { get; set; } //Primary Key Id
        public DateTime FechaPago { get; set; } = DateTime.Now; //Fecha de pago
        public int TipoPagoId { get; set; }//Metodo de pago (Efectivo, Tarjeta, Transferencia, Etc..)
        public bool Activo { get; set; } = true;//Estado del pago (Activo/Inactivo)
        public DateTime FechaFin { get; set; }//Fecha de fin del pago, autocalculado
        //Relacion con la tabla usuarios
        public int UsuarioId { get; set; }
        //Relacion con la tabla Plan
        public int PlanId { get; set; }
        //Nota voy a evitar usar las relaciones para practicar mas las consultas LINQ
    }
}
