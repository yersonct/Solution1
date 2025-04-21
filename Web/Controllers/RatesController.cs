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
    public class RatesController : ControllerBase
    {
        private readonly IRatesService _ratesService;
        private readonly ILogger<RatesController> _logger;

        public RatesController(IRatesService ratesService, ILogger<RatesController> logger)
        {
            _ratesService = ratesService ?? throw new ArgumentNullException(nameof(ratesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRates()
        {
            try
            {
                var rates = await _ratesService.GetAllRatesAsync();
                var result = rates.Select(r => new
                {
                    id = r.id,
                    amount = r.amount,
                    startduration = r.startduration,
                    endduration = r.endduration,
                    id_typerates = r.id_typerates,
                    typerates = r.TypeRates?.name // Asegúrate de que TypeRates no sea nulo
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las tarifas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRatesById(int id)
        {
            try
            {
                var rate = await _ratesService.GetRatesByIdAsync(id);
                if (rate == null)
                {
                    _logger.LogWarning("Tarifa no encontrada con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = rate.id,
                    amount = rate.amount,
                    startduration = rate.startduration,
                    endduration = rate.endduration,
                    id_typerates = rate.id_typerates,
                    typerates = rate.TypeRates?.name
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Rates>> CreateRates([FromBody] RatesDTO ratesDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al crear tarifa.");
                return BadRequest(ModelState);
            }

            try
            {
                var rates = new Rates
                {
                    amount = ratesDTO.amount,
                    startduration = ratesDTO.startduration,
                    endduration = ratesDTO.endduration,
                    id_typerates = ratesDTO.id_typerates,

                };
                var createdRates = await _ratesService.CreateRatesAsync(rates);
                var result = new
                {
                    id = createdRates.id,
                    amount = createdRates.amount,
                    startduration = createdRates.startduration,
                    endduration = createdRates.endduration,
                    id_typerates = createdRates.id_typerates,
                    typerates = createdRates.TypeRates?.name
                };
                return CreatedAtAction(nameof(GetRatesById), new { id = createdRates.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva tarifa.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRates(int id, [FromBody] RatesDTO ratesDTO)
        {
            if (id != ratesDTO.id)
            {
                _logger.LogWarning("Intento de actualizar tarifa con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, ratesDTO.id);
                return BadRequest("El ID de la tarifa no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo no válido al actualizar la tarifa con ID: {Id}", id);
                return BadRequest(ModelState);
            }
            try
            {
                var existingRates = await _ratesService.GetRatesByIdAsync(id);
                if (existingRates == null)
                {
                    _logger.LogWarning("Tarifa no encontrada para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                existingRates.amount = ratesDTO.amount;
                existingRates.startduration = ratesDTO.startduration;
                existingRates.endduration = ratesDTO.endduration;
                existingRates.id_typerates = ratesDTO.id_typerates;

                var result = await _ratesService.UpdateRatesAsync(existingRates);
                if (!result)
                {
                    _logger.LogError("Error al actualizar la tarifa con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRates(int id)
        {
            try
            {
                var result = await _ratesService.DeleteRatesAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Tarifa no encontrada para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarifa con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

