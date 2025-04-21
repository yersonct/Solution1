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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdVehicle = await _vehicleService.CreateVehicleAsync(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.id }, createdVehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            if (id != vehicle.id)
            {
                return BadRequest("El ID del vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.UpdateVehicleAsync(vehicle);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var result = await _vehicleService.DeleteVehicleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}