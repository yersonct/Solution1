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
    public class TypeRatesController : ControllerBase
    {
        private readonly TypeRatesBusiness _TypeRatesBusiness;
        private readonly ILogger<TypeRatesController> _logger;

        public TypeRatesController(TypeRatesBusiness TypeRatesBusiness, ILogger<TypeRatesController> logger)
        {
            _TypeRatesBusiness = TypeRatesBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TypeRatesDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllTypeRatess()
        {
            try
            {
                var TypeRatess = await _TypeRatesBusiness.GetAllTypeRatessAsync();
                return Ok(TypeRatess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los TypeRatesas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TypeRatesDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTypeRatesById(int id)
        {
            try
            {
                var TypeRates = await _TypeRatesBusiness.GetTypeRatesByIdAsync(id);
                return Ok(TypeRates);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para usuario con ID: {TypeRatesId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Usuario no encontrado con ID: {TypeRatesId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {TypeRatesId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TypeRatesDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTypeRatesAsync([FromBody] TypeRatesDTO TypeRatesDTO)
        {
            try
            {
                var createdTypeRates = await _TypeRatesBusiness.CreateTypeRatesAsync(TypeRatesDTO);
                return CreatedAtAction(nameof(GetTypeRatesById), new { id = createdTypeRates.Id }, createdTypeRates);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el TypeRatesas");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el TypeRatesas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TypeRatesDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTypeRatesAsync(int id, [FromBody] TypeRatesDTO TypeRatesDTO)
        {
            if (id != TypeRatesDTO.Id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {


                var updatedTypeRates = await _TypeRatesBusiness.UpdateTypeRatesAsync(TypeRatesDTO);
                return Ok(updatedTypeRates);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el TypeRatesas con ID: {TypeRatesId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "TypeRatesas no encontrado con ID: {TypeRatesId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {TypeRatesId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTypeRatesAsync(int id)
        {
            try
            {
                var deleted = await _TypeRatesBusiness.DeleteTypeRatesAsync(id);
                if (!deleted)
                    return NotFound(new { message = "TypeRatesas no encontrado o ya eliminado" });

                return Ok(new { message = "TypeRatesas eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el TypeRatesas con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
