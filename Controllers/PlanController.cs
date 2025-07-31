using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetAllPlanes")]
        public async Task<IActionResult> GetAllPlanes()
        {
            var planes = await _planRepo.GetAllPlanes();

            if (planes == null || !planes.Any())
            {
                return NotFound(new { message = "No se encontraron Planes." });
            }

            return Ok(planes);
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("GetPlanesById/{id}")]
        public async Task<IActionResult> GetPlanesById(int id)
        {
            var plan = await _planRepo.GetPlanById(id);

            if (plan == null)
            {
                return NotFound(new { message = "Plan no encontrado." });
            }

            return Ok(plan);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        [Route("AddPlan")]
        public async Task<IActionResult> AddPlan([FromBody] Plan plan)
        {
            if (plan == null)
            {
                return BadRequest(new { message = "El plan no puede ser nulo" });
            }

            if (string.IsNullOrEmpty(plan.Nombre))
            {
                return BadRequest(new { message = "El nombre del plan no puede ser nulo" });
            }

            if (plan.Precio <= 0)
            {
                return BadRequest(new { message = "El precio del plan debe ser mayor a 0" });
            }

            if (plan.DuracionDias <= 0)
            {
                return BadRequest(new { message = "La cantidad de dias del plan debe ser mayor a 0" });
            }

            var nuevoPlan = await _planRepo.AddPlan(plan);

            return CreatedAtAction(nameof(GetPlanesById), new { id = nuevoPlan.Id }, nuevoPlan);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("UpdatePlan/{id}")]
        public async Task<IActionResult> UpdatePlan(int id,[FromBody] Plan plan) 
        {
            var planExistente = await _planRepo.GetPlanById(id);

            if (planExistente == null) 
            {
                return NotFound(new { message = "Plan no encontrado." });
            }

            if (plan == null)
            {
                return NotFound(new { message = "Plan no puede ser nulo." });
            }

            planExistente.Nombre = plan.Nombre;
            planExistente.Descripcion = plan.Descripcion;
            planExistente.Precio = plan.Precio;
            planExistente.DuracionDias = plan.DuracionDias;
            planExistente.Activo = plan.Activo;

            var planActualiado = await _planRepo.UpdatePlan(planExistente);

            return Ok(planActualiado);
        }

        [Authorize(Roles = "1")]
        [HttpPut]
        [Route("ChangeStatePlan/{id}")]
        public async Task <IActionResult> ChangeStatePlan(int id, bool activo) 
        {
            var resultado = await _planRepo.ChangeStatePlan(id, activo);

            if (!resultado) 
            {
                return NotFound(new { message = "Tipo de plan no encontrado." });
            }

            return Ok(new { message = "Plan actualizado correctamente" });
        }

        [Authorize(Roles = "1")]
        [HttpDelete]
        [Route("DeletePlan/{id}")]
        public async Task <IActionResult> DeletePlan(int id) 
        {
            var resultado = await _planRepo.DeletePlan(id);

            if (!resultado)
            {
                return NotFound(new { message = "Tipo de plan no encontrado." });
            }

            return Ok(new { message =  "Plan eliminado correctamente" });
        }
    }
}
