using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleHistoriesController : ControllerBase
    {
        private readonly IVehicleHistoryService _vehicleHistoryService;
        private readonly ILogger<VehicleHistoriesController> _logger;

        public VehicleHistoriesController(IVehicleHistoryService vehicleHistoryService, ILogger<VehicleHistoriesController> logger)
        {
            _vehicleHistoryService = vehicleHistoryService ?? throw new ArgumentNullException(nameof(vehicleHistoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllVehicleHistories()
        {
            try
            {
                var vehicleHistories = await _vehicleHistoryService.GetAllVehicleHistoriesAsync();
                var result = vehicleHistories.Select(vh => new
                {
                    id = vh.id,
                    totalTime = vh.TotalTime,
                    registeredVehicleId = vh.RegisteredVehicleId,
                    typeVehicleId = vh.TypeVehicleId
                    //invoiceId = vh.InvoiceId
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los historiales de vehículos.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetVehicleHistoryById(int id)
        {
            try
            {
                var vehicleHistory = await _vehicleHistoryService.GetVehicleHistoryByIdAsync(id);
                if (vehicleHistory == null)
                {
                    _logger.LogWarning("Historial de vehículo no encontrado con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = vehicleHistory.id,
                    totalTime = vehicleHistory.TotalTime,
                    registeredVehicleId = vehicleHistory.RegisteredVehicleId,
                    typeVehicleId = vehicleHistory.TypeVehicleId
                    //invoiceId = vehicleHistory.InvoiceId
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial del vehículo por ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleHistoryDTO>> CreateVehicleHistory([FromBody] VehicleHistoryCreateDTO vehicleHistoryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al crear un nuevo historial de vehículo.");
                return BadRequest(ModelState);
            }

            try
            {
                var createdVehicleHistoryId = await _vehicleHistoryService.CreateVehicleHistoryAsync(vehicleHistoryDTO);

                // Fetch the created entity to return the complete DTO.
                var createdVehicleHistory = await _vehicleHistoryService.GetVehicleHistoryByIdAsync(createdVehicleHistoryId);

                var result = new
                {
                    id = createdVehicleHistory.id,
                    totalTime = createdVehicleHistory.TotalTime,
                    registeredVehicleId = createdVehicleHistory.RegisteredVehicleId,
                    typeVehicleId = createdVehicleHistory.TypeVehicleId
                };
                return CreatedAtAction(nameof(GetVehicleHistoryById), new { id = createdVehicleHistoryId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo historial de vehículo.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleHistory(int id, [FromBody] VehicleHistoryCreateDTO vehicleHistoryDTO)
        {
            if (id != vehicleHistoryDTO.id)
            {
                _logger.LogWarning("Intento de actualizar historial de vehículo con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, vehicleHistoryDTO.id);
                return BadRequest("El ID del historial del vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al actualizar el historial del vehículo con ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _vehicleHistoryService.UpdateVehicleHistoryAsync(id, vehicleHistoryDTO);
                if (!result)
                {
                    _logger.LogWarning("Historial de vehículo no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el historial del vehículo con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleHistory(int id)
        {
            try
            {
                var result = await _vehicleHistoryService.DeleteVehicleHistoryAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Historial de vehículo no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el historial del vehículo con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

    