using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BlackListController : ControllerBase
    {
        private readonly BlackListBusiness _blackListBusiness;
        private readonly ILogger<BlackListController> _logger;

        public BlackListController(BlackListBusiness blackListBusiness, ILogger<BlackListController> logger)
        {
            _blackListBusiness = blackListBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlackListDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _blackListBusiness.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista negra.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlackListDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _blackListBusiness.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Elemento no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el elemento con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(BlackListDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAsync([FromBody] BlackListDTO dto)
        {
            try
            {
                var result = await _blackListBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = result.id }, result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear entrada de lista negra.");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear entrada de lista negra.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BlackListDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BlackListDTO dto)
        {
            if (id != dto.id)
                return BadRequest(new { message = "El ID de la ruta no coincide con el objeto." });

            try
            {
                var updated = await _blackListBusiness.UpdateAsync(dto);
                if (!updated)
                    return NotFound(new { message = "Elemento no encontrado o no se pudo actualizar." });

                return Ok(new { message = "Elemento actualizado correctamente." });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar entrada de lista negra.");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Elemento no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar entrada de lista negra.");
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
                var deleted = await _blackListBusiness.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Elemento no encontrado o ya eliminado." });

                return Ok(new { message = "Elemento eliminado exitosamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar entrada de lista negra.");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
