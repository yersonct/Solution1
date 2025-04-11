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
    public class FormModuleController : ControllerBase
    {
        private readonly FormModuleBusiness _formModuleBusiness;
        private readonly ILogger<FormModuleController> _logger;

        public FormModuleController(FormModuleBusiness formModuleBusiness, ILogger<FormModuleController> logger)
        {
            _formModuleBusiness = formModuleBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFormModules()
        {
            try
            {
                var result = await _formModuleBusiness.GetAllFormModulesAsync();
                return Ok(result);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los FormModules");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormModuleById(int id)
        {
            try
            {
                var result = await _formModuleBusiness.GetFormModuleByIdAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para FormModule con ID: {FormModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "FormModule no encontrado con ID: {FormModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener FormModule con ID: {FormModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFormModuleAsync([FromBody] FormModuleCreateDTO dto)
        {
            try
            {
                var created = await _formModuleBusiness.CreateFormModuleAsync(dto);
                return CreatedAtAction(nameof(GetFormModuleById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear FormModule");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear FormModule");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormModuleAsync(int id, [FromBody] FormModuleCreateDTO dto)
        {
            try
            {
                var updated = await _formModuleBusiness.UpdateFormModuleAsync(id, dto);
                return Ok(new { message = "FormModule actualizado correctamente", updated });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar FormModule con ID: {FormModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "FormModule no encontrado con ID: {FormModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar FormModule con ID: {FormModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteFormModuleAsync(int id)
        {
            try
            {
                var deleted = await _formModuleBusiness.DeleteFormModuleAsync(id);
                if (!deleted)
                    return NotFound(new { message = "FormModule no encontrado o ya eliminado" });

                return Ok(new { message = "FormModule eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar FormModule con ID: {FormModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
