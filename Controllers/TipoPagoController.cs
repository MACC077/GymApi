using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoPagoController : ControllerBase
    {
        private readonly ITipoPago _tipoPagoRepo;

        public TipoPagoController(ITipoPago tipoPagoRepo)
        {
            _tipoPagoRepo = tipoPagoRepo;
        }

        [HttpGet]
        [Route("GetAllTipoPagos")]
        public async Task<IActionResult> GetAllTipoPagos()
        {
            var tipoPagos = await _tipoPagoRepo.GetAllTipoPagos();

            if (tipoPagos == null || !tipoPagos.Any())
            {
                return NotFound("No se encontraron tipos de pago.");
            }

            return Ok(tipoPagos);
        }

        [HttpGet]
        [Route("GetTipoPagoById/{id}")]
        public async Task<IActionResult> GetTipoPagoById(int id)
        {
            var tipoPago = await _tipoPagoRepo.GetTipoPagoById(id);

            if (tipoPago == null)
            {
                return NotFound("Tipo de pago no encontrado.");
            }

            return Ok(tipoPago);
        }

        [HttpPost]
        [Route("AddTipoPago")]
        public async Task<IActionResult> AddTipoPago([FromBody] TipoPago tipoPago)
        {
            if (tipoPago == null)
            {
                return BadRequest("El tipo de pago no puede ser nulo.");
            }

            if (string.IsNullOrEmpty(tipoPago.Nombre))
            {
                return BadRequest("El nombre del tipo de pago es obligatorio.");
            }

            var nuevoTipoPago = await _tipoPagoRepo.AddTipoPago(tipoPago);

            return CreatedAtAction(nameof(GetTipoPagoById), new { id = nuevoTipoPago.Id }, nuevoTipoPago);
        }

        [HttpPut]
        [Route("UpdateTipoPago/{id}")]
        public async Task<IActionResult> UpdateTipoPago(int id,[FromBody] TipoPago tipoPago)
        {
            var tipoPagoExistente = await _tipoPagoRepo.GetTipoPagoById(id);

            if (tipoPagoExistente == null)
            {
                return NotFound("Tipo de pago no encontrado.");
            }

            if (tipoPago == null)
            {
                return BadRequest("El tipo de pago no puede ser nulo.");
            }

            if (string.IsNullOrEmpty(tipoPago.Nombre))
            {
                return BadRequest("El nombre del tipo de pago es obligatorio.");
            }

            tipoPagoExistente.Nombre = tipoPago.Nombre;
            tipoPagoExistente.Activo = tipoPago.Activo;

            var tipoPagoActualizado = await _tipoPagoRepo.UpdateTipoPago(tipoPagoExistente);
            return Ok(tipoPagoActualizado);
        }

        [HttpPut]
        [Route("ChangeStateTipoPago/{id}")]
        public async Task<IActionResult> ChangeStateTipoPago(int id, [FromBody] bool activo)
        {
            var resultado = await _tipoPagoRepo.ChangeStateTipoPago(id, activo);

            if (!resultado)
            {
                return NotFound("Tipo de pago no encontrado.");
            }

            return Ok("Tipo de Pago actualizado correctamente");
        }

        [HttpDelete]
        [Route("DeleteTipoPago/{id}")]
        public async Task<IActionResult> DeleteTipoPago(int id)
        {
            var resultado = await _tipoPagoRepo.DeleteTipoPago(id);

            if (!resultado)
            {
                return NotFound("Tipo de pago no encontrado.");
            }

            return Ok("Tipo de Pago eliminado correctamente");
        }
    }
}
