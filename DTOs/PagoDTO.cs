namespace GymControlAPI.DTOs
{
    public class PagoDTO
    {
        public int? Id { get; set; } //Primary Key Id
        public DateTime FechaPago { get; set; } = DateTime.Now; //Fecha de pago
        public int? TipoPagoId { get; set; }//Metodo de pago (Efectivo, Tarjeta, Transferencia, Etc..)
        public string TipoPagoNombre { get; set; } //Nombre del metodo de pago
        public bool Activo { get; set; } = true;//Estado del pago (Activo/Inactivo)
        public DateTime FechaFin { get; set; }//Fecha de fin del pago, autocalculado
        //Relacion con la tabla usuarios
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } //Nombre del usuario
        //Relacion con la tabla Plan
        public int? PlanId { get; set; }
        public string PlanNombre { get; set; } //Nombre del plan
        //Nota voy a evitar usar las relaciones en el modelo para practicar mas las consultas LINQ
    }
}
