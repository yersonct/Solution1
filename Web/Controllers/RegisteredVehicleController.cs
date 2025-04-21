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
    public class RegisteredVehiclesController : ControllerBase
    {
        private readonly IRegisteredVehicleService _registeredVehicleService;
        private readonly ILogger<RegisteredVehiclesController> _logger;

        public RegisteredVehiclesController(IRegisteredVehicleService registeredVehicleService, ILogger<RegisteredVehiclesController> logger)
        {
            _registeredVehicleService = registeredVehicleService ?? throw new ArgumentNullException(nameof(registeredVehicleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRegisteredVehicles()
        {
            try
            {
                var registeredVehicles = await _registeredVehicleService.GetAllRegisteredVehiclesAsync();
                var result = registeredVehicles.Select(rv => new
                {
                    id = rv.id,
                    id_vehicle = rv.id_vehicle,
                    entrydatetime = rv.entrydatetime,
                    exitdatetime = rv.exitdatetime
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los vehículos registrados.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRegisteredVehicleById(int id)
        {
            try
            {
                var registeredVehicle = await _registeredVehicleService.GetRegisteredVehicleByIdAsync(id);
                if (registeredVehicle == null)
                {
                    _logger.LogWarning("Vehículo registrado no encontrado con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = registeredVehicle.id,
                    id_vehicle = registeredVehicle.id_vehicle,
                    entrydatetime = registeredVehicle.entrydatetime,
                    exitdatetime = registeredVehicle.exitdatetime
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo registrado por ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RegisteredVehicle>> CreateRegisteredVehicle([FromBody] RegisteredVehicleCreateDTO registeredVehicleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al crear un vehículo registrado.");
                return BadRequest(ModelState);
            }
            try
            {
                var registeredVehicle = new RegisteredVehicle
                {
                    entrydatetime = registeredVehicleDTO.entrydatetime,
                    exitdatetime = registeredVehicleDTO.exitdatetime,
                    id_vehicle = registeredVehicleDTO.id_vehicle,
                };

                var createdRegisteredVehicle = await _registeredVehicleService.CreateRegisteredVehicleAsync(registeredVehicle);
                var result = new
                {
                    id = createdRegisteredVehicle.id,
                    id_vehicle = createdRegisteredVehicle.id_vehicle,
                    entrydatetime = createdRegisteredVehicle.entrydatetime,
                    exitdatetime = createdRegisteredVehicle.exitdatetime
                };
                return CreatedAtAction(nameof(GetRegisteredVehicleById), new { id = createdRegisteredVehicle.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo vehículo registrado.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegisteredVehicle(int id, [FromBody] RegisteredVehicleDTO registeredVehicleDTO)
        {
            if (id != registeredVehicleDTO.id)
            {
                _logger.LogWarning("Intento de actualizar vehículo registrado con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, registeredVehicleDTO.id);
                return BadRequest("El ID del vehículo registrado no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al actualizar el vehículo registrado con ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var existingRegisteredVehicle = await _registeredVehicleService.GetRegisteredVehicleByIdAsync(id);
                if (existingRegisteredVehicle == null)
                {
                    _logger.LogWarning("Vehículo registrado no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                existingRegisteredVehicle.entrydatetime = registeredVehicleDTO.entrydatetime;
                existingRegisteredVehicle.exitdatetime = registeredVehicleDTO.exitdatetime;
                existingRegisteredVehicle.id_vehicle = registeredVehicleDTO.id_vehicle;


                var result = await _registeredVehicleService.UpdateRegisteredVehicleAsync(existingRegisteredVehicle);
                if (!result)
                {
                    _logger.LogError("Error al actualizar el vehículo registrado con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el vehículo registrado con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredVehicle(int id)
        {
            try
            {
                var result = await _registeredVehicleService.DeleteRegisteredVehicleAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Vehículo registrado no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el vehículo registrado con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
