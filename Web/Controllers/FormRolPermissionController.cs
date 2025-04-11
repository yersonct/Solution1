using Business;
using Entity.DTOs;
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
    public class FormRolPermissionController : ControllerBase
    {
        private readonly FormRolPermissionBusiness _business;
        private readonly ILogger<FormRolPermissionController> _logger;

        public FormRolPermissionController(FormRolPermissionBusiness business, ILogger<FormRolPermissionController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormRolPermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _business.GetAllAsync();
                return Ok(result);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las relaciones FormRolPermission.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormRolPermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _business.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "ID inválido: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Relación no encontrada: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al obtener el ID {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormRolPermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAsync([FromBody] FormRolPermissionCreateDTO dto)
        {
            try
            {
                var created = await _business.CreateAsync(dto);

                if (created == null || created.Id == 0)
                {
                    _logger.LogError("El objeto creado es nulo o tiene un Id inválido.");
                    return StatusCode(500, new { message = "Error al crear la relación." });
                }

                return CreatedAtAction(
                    nameof(GetByIdAsync),
                    new { id = created.Id },
                    created
                );
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear la relación.");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al crear la relación.");
                return StatusCode(500, new { message = $"Error en el servicio externo 'Base de datos': {ex.Message}" });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(FormRolPermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAsync([FromBody] FormRolPermissionDTO dto)
        {
            try
            {
                var updated = await _business.UpdateAsync(dto);
                if (!updated)
                    return NotFound(new { message = "Relación no encontrada o no se pudo actualizar." });

                return Ok(dto);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar la relación con ID: {Id}", dto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Relación no encontrada para actualizar: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al actualizar la relación con ID: {Id}", dto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var deleted = await _business.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Relación no encontrada o ya eliminada." });

                return Ok(new { message = "Relación eliminada correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error interno al eliminar la relación con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
