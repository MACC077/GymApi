using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetAllPagos")]
        public async Task<IActionResult> GetAllPagos()
        {
            var pagos = await _pagoRepo.GetAllPago();

            if (pagos == null || !pagos.Any())
            {
                return NotFound(new { message = "No se encontraron Pagos." });
            }

            return Ok(pagos);
        }

        [Authorize(Roles = "1,4")]
        [HttpGet]
        [Route("GetPagosById/{id}")]
        public async Task<IActionResult> GetPagosById(int id)
        {
            var pago = await _pagoRepo.GetPagoById(id);

            if (pago == null)
            {
                return NotFound(new { message = "Pago no encontrado." });
            }

            return Ok(pago);
        }

        [Authorize(Roles = "1,4")]
        [HttpPost]
        [Route("AddPago")]
        public async Task<IActionResult> AddPago([FromBody] Pago pago)
        {
            if (pago == null)
            {
                return BadRequest(new { message = "El pago no puede ser nulo." });
            }

            if (pago.UsuarioId <= 0 || pago.PlanId <= 0 || pago.TipoPagoId <= 0)
            {
                return BadRequest(new { message = "El UsuarioId, PlanId y TipoPagoId son obligatorios." });
            }

            var nuevoPago = await _pagoRepo.AddPago(pago);

            if (nuevoPago == null)
            {
                // Devuelve un error 500 si no se pudo crear el recurso
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al crear el pago" });
            }
            // Devuelve un 201 Created si el recurso se creó correctamente
            return CreatedAtAction(nameof(GetPagosById), new { id = nuevoPago.Id }, nuevoPago);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("UpdatePago/{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] Pago pago)
        {
            var pagoExistente = await _pagoRepo.GetPagoById(id);

            if (pagoExistente == null)
            {
                return NotFound(new { message = "Pago no encontrado." });
            }

            if (pago == null)
            {
                return BadRequest(new { message = "El pago no puede ser nulo." });
            }
           
            if (pago.UsuarioId <= 0 || pago.PlanId <= 0 || pago.TipoPagoId <= 0)
            {
                return BadRequest(new { message = "El UsuarioId, PlanId y TipoPagoId son obligatorios." });
            }

            var resultado = await _pagoRepo.UpdatePago(pago);
            return Ok(resultado);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("ChangeStatePago/{id}")]
        public async Task<IActionResult> ChangeStatePago(int id, [FromBody] bool estado)
        {
        
            var resultado = await _pagoRepo.ChangeStatePago(id, estado);

            if (!resultado)
            {
                return BadRequest(new { message = "No se pudo cambiar el estado del pago." });
            }

            return Ok(new { message = "Pago actualizado correctamente." });
        }

        [Authorize(Roles = "1")]
        [HttpDelete]
        [Route("DeletePago/{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pagoExistente = await _pagoRepo.GetPagoById(id);
            if (pagoExistente == null)
            {
                return NotFound(new { message = "Pago no encontrado." });
            }
            var resultado = await _pagoRepo.DeletePago(id);
            if (!resultado)
            {
                return BadRequest(new { message = "No se pudo eliminar el pago." });
            }

            return NoContent();
        }
    }
}
