using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeRatesController : ControllerBase
    {
        private readonly ITypeRatesService _typeRatesService;
        private readonly ILogger<TypeRatesController> _logger;

        public TypeRatesController(ITypeRatesService typeRatesService, ILogger<TypeRatesController> logger)
        {
            _typeRatesService = typeRatesService ?? throw new ArgumentNullException(nameof(typeRatesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTypeRates()
        {
            try
            {
                var typeRates = await _typeRatesService.GetAllTypeRatesAsync();
                var result = typeRates.Select(tr => new
                {
                    id = tr.id,
                    name = tr.name,
                    price = tr.price,
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de tarifas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTypeRatesById(int id)
        {
            try
            {
                var typeRates = await _typeRatesService.GetTypeRatesByIdAsync(id);
                if (typeRates == null)
                {
                    _logger.LogWarning("Tipo de tarifa no encontrado con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = typeRates.id,
                    name = typeRates.name,
                    price = typeRates.price,
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TypeRates>> CreateTypeRates([FromBody] TypeRatesDTO typeRatesDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al crear tipo de tarifa.");
                return BadRequest(ModelState);
            }

            try
            {
                var typeRates = new TypeRates
                {
                    name = typeRatesDTO.name,
                    price = typeRatesDTO.price,
                };

                var createdTypeRates = await _typeRatesService.CreateTypeRatesAsync(typeRates);
                return CreatedAtAction(nameof(GetTypeRatesById), new { id = createdTypeRates.id }, createdTypeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo tipo de tarifa.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeRates(int id, [FromBody] TypeRatesDTO typeRatesDTO)
        {
            if (id != typeRatesDTO.id)
            {
                _logger.LogWarning("Intento de actualizar tipo de tarifa con ID no coincidente.  Ruta ID: {RouteId}, DTO ID: {DtoId}", id, typeRatesDTO.id);
                return BadRequest("El ID del tipo de tarifa no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al actualizar tipo de tarifa con ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var existingTypeRates = await _typeRatesService.GetTypeRatesByIdAsync(id);
                if (existingTypeRates == null)
                {
                    _logger.LogWarning("Tipo de tarifa no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }

                existingTypeRates.name = typeRatesDTO.name;
                existingTypeRates.price = typeRatesDTO.price;


                var result = await _typeRatesService.UpdateTypeRatesAsync(existingTypeRates);
                if (!result)
                {
                    _logger.LogError("Error al actualizar el tipo de tarifa con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeRates(int id)
        {
            try
            {
                var result = await _typeRatesService.DeleteTypeRatesAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Tipo de tarifa no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
