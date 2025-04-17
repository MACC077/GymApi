using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPago _pagoRepo;

        public PagoController(IPago pagoRepo)
        {
            _pagoRepo = pagoRepo;
        }

        [HttpGet]
        [Route("GetAllPagos")]
        public async Task<IActionResult> GetAllPagos()
        {
            var pagos = await _pagoRepo.GetAllPago();

            if (pagos == null || !pagos.Any())
            {
                return NotFound("No se encontraron Pagos.");
            }

            return Ok(pagos);
        }

        [HttpGet]
        [Route("GetPagosById/{id}")]
        public async Task<IActionResult> GetPagosById(int id)
        {
            var pago = await _pagoRepo.GetPagoById(id);

            if (pago == null)
            {
                return NotFound("Pago no encontrado.");
            }

            return Ok(pago);
        }

        [HttpPost]
        [Route("AddPago")]
        public async Task<IActionResult> AddPago([FromBody] Pago pago)
        {
            if (pago == null)
            {
                return BadRequest("El pago no puede ser nulo.");
            }

            if (pago.UsuarioId <= 0 || pago.PlanId <= 0 || pago.TipoPagoId <= 0)
            {
                return BadRequest("El UsuarioId, PlanId y TipoPagoId son obligatorios.");
            }

            var nuevoPago = await _pagoRepo.AddPago(pago);

            if (nuevoPago == null)
            {
                // Devuelve un error 500 si no se pudo crear el recurso
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el pago");
            }
            // Devuelve un 201 Created si el recurso se creó correctamente
            return CreatedAtAction(nameof(GetPagosById), new { id = nuevoPago.Id }, nuevoPago);
        }

        [HttpPut]
        [Route("UpdatePago/{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] Pago pago)
        {
            var pagoExistente = await _pagoRepo.GetPagoById(id);

            if (pagoExistente == null)
            {
                return NotFound("Pago no encontrado.");
            }

            if (pago == null)
            {
                return BadRequest("El pago no puede ser nulo.");
            }
           
            if (pago.UsuarioId <= 0 || pago.PlanId <= 0 || pago.TipoPagoId <= 0)
            {
                return BadRequest("El UsuarioId, PlanId y TipoPagoId son obligatorios.");
            }

            var resultado = await _pagoRepo.UpdatePago(pago);
            return Ok(resultado);
        }

        [HttpPut]
        [Route("ChangeStatePago/{id}")]
        public async Task<IActionResult> ChangeStatePago(int id, [FromBody] bool estado)
        {
        
            var resultado = await _pagoRepo.ChangeStatePago(id, estado);

            if (!resultado)
            {
                return BadRequest("No se pudo cambiar el estado del pago.");
            }

            return Ok("Pago actualizado correctamente.");
        }

        [HttpDelete]
        [Route("DeletePago/{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pagoExistente = await _pagoRepo.GetPagoById(id);
            if (pagoExistente == null)
            {
                return NotFound("Pago no encontrado.");
            }
            var resultado = await _pagoRepo.DeletePago(id);
            if (!resultado)
            {
                return BadRequest("No se pudo eliminar el pago.");
            }

            return NoContent();
        }
    }
}
