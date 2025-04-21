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
    public class RegisteredVehiclesController : ControllerBase
    {
        private readonly IRegisteredVehicleService _registeredVehicleService;

        public RegisteredVehiclesController(IRegisteredVehicleService registeredVehicleService)
        {
            _registeredVehicleService = registeredVehicleService ?? throw new ArgumentNullException(nameof(registeredVehicleService));
        }

        [HttpGet("CanConnect")]
        public async Task<ActionResult<bool>> CanConnect()
        {
            var canConnect = await _registeredVehicleService.CanConnectAsync();
            return Ok(canConnect);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredVehicle>>> GetAllRegisteredVehicles()
        {
            var registeredVehicles = await _registeredVehicleService.GetAllRegisteredVehiclesAsync();
            return Ok(registeredVehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredVehicle>> GetRegisteredVehicleById(int id)
        {
            var registeredVehicle = await _registeredVehicleService.GetRegisteredVehicleByIdAsync(id);
            if (registeredVehicle == null)
            {
                return NotFound();
            }
            return Ok(registeredVehicle);
        }

        [HttpPost]
        public async Task<ActionResult<RegisteredVehicle>> CreateRegisteredVehicle([FromBody] RegisteredVehicle registeredVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRegisteredVehicle = await _registeredVehicleService.CreateRegisteredVehicleAsync(registeredVehicle);
            return CreatedAtAction(nameof(GetRegisteredVehicleById), new { id = createdRegisteredVehicle.id }, createdRegisteredVehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegisteredVehicle(int id, [FromBody] RegisteredVehicle registeredVehicle)
        {
            if (id != registeredVehicle.id)
            {
                return BadRequest("El ID del vehículo registrado no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _registeredVehicleService.UpdateRegisteredVehicleAsync(registeredVehicle);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredVehicle(int id)
        {
            var result = await _registeredVehicleService.DeleteRegisteredVehicleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}