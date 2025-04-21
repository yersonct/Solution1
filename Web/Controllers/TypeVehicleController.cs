using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeVehiclesController : ControllerBase
    {
        private readonly ITypeVehicleService _typeVehicleService;

        public TypeVehiclesController(ITypeVehicleService typeVehicleService)
        {
            _typeVehicleService = typeVehicleService ?? throw new ArgumentNullException(nameof(typeVehicleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeVehicle>>> GetAllTypeVehicles()
        {
            var typeVehicles = await _typeVehicleService.GetAllTypeVehiclesAsync();
            return Ok(typeVehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeVehicle>> GetTypeVehicleById(int id)
        {
            var typeVehicle = await _typeVehicleService.GetTypeVehicleByIdAsync(id);
            if (typeVehicle == null)
            {
                return NotFound();
            }
            return Ok(typeVehicle);
        }

        [HttpPost]
        public async Task<ActionResult<TypeVehicle>> CreateTypeVehicle([FromBody] TypeVehicle typeVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTypeVehicle = await _typeVehicleService.CreateTypeVehicleAsync(typeVehicle);
            return CreatedAtAction(nameof(GetTypeVehicleById), new { id = createdTypeVehicle.id }, createdTypeVehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeVehicle(int id, [FromBody] TypeVehicle typeVehicle)
        {
            if (id != typeVehicle.id)
            {
                return BadRequest("El ID del tipo de vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _typeVehicleService.UpdateTypeVehicleAsync(typeVehicle);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeVehicle(int id)
        {
            var result = await _typeVehicleService.DeleteTypeVehicleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}