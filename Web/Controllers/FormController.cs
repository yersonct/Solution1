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
    public class FormsController : ControllerBase
    {
        private readonly FormBusiness _formsBusiness;
        private readonly ILogger<FormsController> _logger;

        public FormsController(FormBusiness formsBusiness, ILogger<FormsController> logger)
        {
            _formsBusiness = formsBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var forms = await _formsBusiness.GetAllFormsAsync();
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los forms");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var forms = await _formsBusiness.GetFormsByIdAsync(id);
                return Ok(forms);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el modulos con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModuleAsync([FromBody] FormDTO formDTO)
        {
            try
            {
                var createdForm = await _formsBusiness.CreateFormsAsync(formDTO);
                return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el Personas");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Personas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormAsync(int id, [FromBody] FormDTO formDTO)
        {
            if (id != formDTO.Id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {


                var updatedForm = await _formsBusiness.UpdateFormAsync(formDTO);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Personas no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteFormAsync(int id)
        {
            try
            {
                var deleted = await _formsBusiness.DeleteFormsAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Personas no encontrado o ya eliminado" });

                return Ok(new { message = "Personas eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Personas con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
