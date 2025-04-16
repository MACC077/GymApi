using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GymControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlan _planRepo;

        public PlanController(IPlan planRepo)
        {
            _planRepo = planRepo;
        }

        [HttpGet]
        [Route("GetAllPlanes")]
        public async Task<IActionResult> GetAllPlanes()
        {
            var planes = await _planRepo.GetAllPlanes();

            if (planes == null || !planes.Any())
            {
                return NotFound("No se encontraron Planes.");
            }

            return Ok(planes);
        }

        [HttpGet]
        [Route("GetPlanesById/{id}")]
        public async Task<IActionResult> GetPlanesById(int id)
        {
            var plan = await _planRepo.GetPlanById(id);

            if (plan == null)
            {
                return NotFound("Plan no encontrado.");
            }

            return Ok(plan);
        }

        [HttpPost]
        [Route("AddPlan")]
        public async Task<IActionResult> AddPlan([FromBody] Plan plan)
        {
            if (plan == null)
            {
                return BadRequest("El plan no puede ser nulo");
            }

            if (string.IsNullOrEmpty(plan.Nombre))
            {
                return BadRequest("El nombre del plan no puede ser nulo");
            }

            if (plan.Precio <= 0)
            {
                return BadRequest("El precio del plan debe ser mayor a 0");
            }

            if (plan.DuracionDias <= 0)
            {
                return BadRequest("La cantidad de dias del plan debe ser mayor a 0");
            }

            var nuevoPlan = await _planRepo.AddPlan(plan);

            return CreatedAtAction(nameof(GetPlanesById), new { id = nuevoPlan.Id }, nuevoPlan);
        }

        [HttpPut]
        [Route("UpdatePlan/{id}")]
        public async Task<IActionResult> UpdatePlan(int id,[FromBody] Plan plan) 
        {
            var planExistente = await _planRepo.GetPlanById(id);

            if (planExistente == null) 
            {
                return NotFound("Plan no encontrado.");
            }

            if (plan == null)
            {
                return NotFound("Plan no puede ser nulo.");
            }

            planExistente.Nombre = plan.Nombre;
            planExistente.Descripcion = plan.Descripcion;
            planExistente.Precio = plan.Precio;
            planExistente.DuracionDias = plan.DuracionDias;
            planExistente.Activo = plan.Activo;

            var planActualiado = await _planRepo.UpdatePlan(planExistente);

            return Ok(planActualiado);
        }

        [HttpPut]
        [Route("ChangeStatePlan/{id}")]
        public async Task <IActionResult> ChangeStatePlan(int id, bool activo) 
        {
            var resultado = await _planRepo.ChangeStatePlan(id, activo);

            if (!resultado) 
            {
                return NotFound("Tipo de plan no encontrado.");
            }

            return Ok("Plan actualizado correctamente");
        }

        [HttpDelete]
        [Route("DeletePlan/{id}")]
        public async Task <IActionResult> DeletePlan(int id) 
        {
            var resultado = await _planRepo.DeletePlan(id);

            if (!resultado)
            {
                return NotFound("Tipo de plan no encontrado.");
            }

            return Ok("Plan eliminado correctamente");
        }
    }
}
