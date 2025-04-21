using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleHistoriesController : ControllerBase
    {
        private readonly IVehicleHistoryService _vehicleHistoryService;

        public VehicleHistoriesController(IVehicleHistoryService vehicleHistoryService)
        {
            _vehicleHistoryService = vehicleHistoryService ?? throw new ArgumentNullException(nameof(vehicleHistoryService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleHistoryDTO>>> GetAllVehicleHistories()
        {
            var vehicleHistories = await _vehicleHistoryService.GetAllVehicleHistoriesAsync();
            return Ok(vehicleHistories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleHistoryDTO>> GetVehicleHistoryById(int id)
        {
            var vehicleHistory = await _vehicleHistoryService.GetVehicleHistoryByIdAsync(id);
            if (vehicleHistory == null)
            {
                return NotFound();
            }
            return Ok(vehicleHistory);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateVehicleHistory([FromBody] VehicleHistoryCreateDTO vehicleHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdVehicleHistoryId = await _vehicleHistoryService.CreateVehicleHistoryAsync(vehicleHistory);
            return CreatedAtAction(nameof(GetVehicleHistoryById), new { id = createdVehicleHistoryId }, createdVehicleHistoryId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleHistory(int id, [FromBody] VehicleHistoryCreateDTO vehicleHistory)
        {
            if (id != vehicleHistory.id)
            {
                return BadRequest("El ID del historial del vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleHistoryService.UpdateVehicleHistoryAsync(id, vehicleHistory);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleHistory(int id)
        {
            var result = await _vehicleHistoryService.DeleteVehicleHistoryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}