using Business;
using Entity.DTOs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RatesController : ControllerBase
    {
        private readonly RatesBusiness _ratesBusiness;
        private readonly ILogger<RatesController> _logger;

        public RatesController(RatesBusiness ratesBusiness, ILogger<RatesController> logger)
        {
            _ratesBusiness = ratesBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RatesDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRates()
        {
            try
            {
                var rates = await _ratesBusiness.GetAllRatessAsync();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los rates");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RatesDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRatesById(int id)
        {
            try
            {
                var rate = await _ratesBusiness.GetRatesByIdAsync(id);
                return Ok(rate);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para rate con ID: {RatesId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rate no encontrado con ID: {RatesId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el rate con ID: {RatesId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RatesDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRatesAsync([FromBody] RatesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdRate = await _ratesBusiness.CreateRatesAsync(dto);
                return CreatedAtAction(nameof(GetRatesById), new { id = createdRate.id }, createdRate);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el rate");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el rate");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RatesDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRatesAsync(int id, [FromBody] RatesDTO dto)
        {
            if (id != dto.id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedRate = await _ratesBusiness.UpdateRatesAsync(dto);
                return Ok(updatedRate);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el rate con ID: {RatesId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rate no encontrado con ID: {RatesId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el rate con ID: {RatesId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRatesAsync(int id)
        {
            try
            {
                var deleted = await _ratesBusiness.DeleteRatesAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Rate no encontrado o ya eliminado" });

                return Ok(new { message = "Rate eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el rate con ID: {RatesId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
