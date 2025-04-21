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
    public class TypeVehiclesController : ControllerBase
    {
        private readonly ITypeVehicleService _typeVehicleService;
        private readonly ILogger<TypeVehiclesController> _logger;

        public TypeVehiclesController(ITypeVehicleService typeVehicleService, ILogger<TypeVehiclesController> logger)
        {
            _typeVehicleService = typeVehicleService ?? throw new ArgumentNullException(nameof(typeVehicleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTypeVehicles()
        {
            try
            {
                var typeVehicles = await _typeVehicleService.GetAllTypeVehiclesAsync();
                var result = typeVehicles.Select(tv => new
                {
                    id = tv.id,
                    name = tv.name,
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de vehículos.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTypeVehicleById(int id)
        {
            try
            {
                var typeVehicle = await _typeVehicleService.GetTypeVehicleByIdAsync(id);
                if (typeVehicle == null)
                {
                    _logger.LogWarning("Tipo de vehículo no encontrado con ID: {Id}", id);
                    return NotFound();
                }

                var result = new
                {
                    id = typeVehicle.id,
                    name = typeVehicle.name,
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de vehículo con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TypeVehicle>> CreateTypeVehicle([FromBody] TypeVehicleDTO typeVehicleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al crear tipo de vehículo.");
                return BadRequest(ModelState);
            }
            try
            {
                var typeVehicle = new TypeVehicle
                {
                    name = typeVehicleDTO.name,
                };

                var createdTypeVehicle = await _typeVehicleService.CreateTypeVehicleAsync(typeVehicle);
                var result = new
                {
                    id = createdTypeVehicle.id,
                    name = createdTypeVehicle.name,
                };
                return CreatedAtAction(nameof(GetTypeVehicleById), new { id = createdTypeVehicle.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo tipo de vehículo.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeVehicle(int id, [FromBody] TypeVehicleDTO typeVehicleDTO)
        {
            if (id != typeVehicleDTO.id)
            {
                _logger.LogWarning("Intento de actualizar tipo de vehículo con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, typeVehicleDTO.id);
                return BadRequest("El ID del tipo de vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al actualizar el tipo de vehículo con ID: {Id}", id);
                return BadRequest(ModelState);
            }
            try
            {
                var existingTypeVehicle = await _typeVehicleService.GetTypeVehicleByIdAsync(id);
                if (existingTypeVehicle == null)
                {
                    _logger.LogWarning("Tipo de vehículo no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }

                existingTypeVehicle.name = typeVehicleDTO.name;


                var result = await _typeVehicleService.UpdateTypeVehicleAsync(existingTypeVehicle);
                if (!result)
                {
                    _logger.LogError("Error al actualizar el tipo de vehículo con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de vehículo con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeVehicle(int id)
        {
            try
            {
                var result = await _typeVehicleService.DeleteTypeVehicleAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Tipo de vehículo no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de vehículo con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

