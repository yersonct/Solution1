using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleHistoryParkingRatesController : ControllerBase
    {
        private readonly IVehicleHistoryParkingRatesService _vhprService;
        private readonly ILogger<VehicleHistoryParkingRatesController> _logger;

        public VehicleHistoryParkingRatesController(IVehicleHistoryParkingRatesService vhprService, ILogger<VehicleHistoryParkingRatesController> logger)
        {
            _vhprService = vhprService ?? throw new ArgumentNullException(nameof(vhprService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllVehicleHistoryParkingRates()
        {
            try
            {
                var result = await _vhprService.GetAllVehicleHistoryParkingRatesAsync();
                var formattedResult = result.Select(vhpr => new
                {
                    id = vhpr.id,
                    vehicleHistoryId = vhpr.id_vehiclehistory,
                    ratesId = vhpr.id_rates,
                    parkingId = vhpr.id_parking,
                    hoursUsed = vhpr.hourused,
                    subTotal = vhpr.subtotal
                });
                return Ok(formattedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de tarifas de estacionamiento.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetVehicleHistoryParkingRatesById(int id)
        {
            try
            {
                var result = await _vhprService.GetVehicleHistoryParkingRatesByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("Registro de tarifa de estacionamiento no encontrado con ID: {Id}", id);
                    return NotFound();
                }
                var formattedResult = new
                {
                    id = result.id,
                    vehicleHistoryId = result.id_vehiclehistory,
                    ratesId = result.id_rates,
                    parkingId = result.id_parking,
                    hoursUsed = result.hourused,
                    subTotal = result.subtotal
                };
                return Ok(formattedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de tarifa de estacionamiento con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleHistoryParkingRates>> CreateVehicleHistoryParkingRates([FromBody] VehicleHistoryParkingRatesDTO vhprDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al crear un nuevo registro de tarifa de estacionamiento.");
                return BadRequest(ModelState);
            }
            try
            {
                var vhpr = new VehicleHistoryParkingRates
                {
                    id_vehiclehistory = vhprDTO.VehicleHistoryId,
                    id_rates = vhprDTO.RatesId,
                    id_parking = vhprDTO.ParkingId,
                    hourused = vhprDTO.HoursUsed,
                    subtotal = vhprDTO.SubTotal
                };

                var created = await _vhprService.CreateVehicleHistoryParkingRatesAsync(vhpr);
                var result = new
                {
                    id = created.id,
                    vehicleHistoryId = created.id_vehiclehistory,
                    ratesId = created.id_rates,
                    parkingId = created.id_parking,
                    hoursUsed = created.hourused,
                    subTotal = created.subtotal
                };
                return CreatedAtAction(nameof(GetVehicleHistoryParkingRatesById), new { id = created.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de tarifa de estacionamiento.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleHistoryParkingRates(int id, [FromBody] VehicleHistoryParkingRatesDTO vhprDTO)
        {
            if (id != vhprDTO.Id)
            {
                _logger.LogWarning("Intento de actualizar registro de tarifa de estacionamiento con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, vhprDTO.Id);
                return BadRequest("El ID de VehicleHistoryParkingRates no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al actualizar el registro de tarifa de estacionamiento con ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var existingVhpr = await _vhprService.GetVehicleHistoryParkingRatesByIdAsync(id);
                if (existingVhpr == null)
                {
                    _logger.LogWarning("Registro de tarifa de estacionamiento no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }

                existingVhpr.id_vehiclehistory = vhprDTO.VehicleHistoryId;
                existingVhpr.id_rates = vhprDTO.RatesId;
                existingVhpr.id_parking = vhprDTO.ParkingId;
                existingVhpr.hourused = vhprDTO.HoursUsed;
                existingVhpr.subtotal = vhprDTO.SubTotal;

                var result = await _vhprService.UpdateVehicleHistoryParkingRatesAsync(existingVhpr);
                if (!result)
                {
                    _logger.LogError("Error al actualizar el registro de tarifa de estacionamiento con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de tarifa de estacionamiento con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleHistoryParkingRates(int id)
        {
            try
            {
                var result = await _vhprService.DeleteVehicleHistoryParkingRatesAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Registro de tarifa de estacionamiento no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de tarifa de estacionamiento con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

