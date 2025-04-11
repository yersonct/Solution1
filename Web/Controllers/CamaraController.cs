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
    public class CamaraController : ControllerBase
    {
        private readonly CamaraBusiness _CamaraBusiness;
        private readonly ILogger<CamaraController> _logger;

        public CamaraController(CamaraBusiness CamaraBusiness, ILogger<CamaraController> logger)
        {
            _CamaraBusiness = CamaraBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CamaraDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCamaras()
        {
            try
            {
                var Camaras = await _CamaraBusiness.GetAllCamarasAsync();
                return Ok(Camaras);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Camaras");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CamaraDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCamaraById(int id)
        {
            try
            {
                var Camara = await _CamaraBusiness.GetCamaraByIdAsync(id);
                return Ok(Camara);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para usuario con ID: {CamaraId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Usuario no encontrado con ID: {CamaraId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {CamaraId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CamaraDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCamaraAsync([FromBody] CamaraDTO CamaraDTO)
        {
            try
            {
                var createdCamara = await _CamaraBusiness.CreateCamaraAsync(CamaraDTO);
                return CreatedAtAction(nameof(GetCamaraById), new { id = createdCamara.id }, createdCamara);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el Camaraas");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Camaraas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CamaraDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCamaraAsync(int id, [FromBody] CamaraDTO CamaraDTO)
        {
            if (id != CamaraDTO.id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {


                var updatedCamara = await _CamaraBusiness.UpdateCamaraAsync(CamaraDTO);
                return Ok(updatedCamara);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el Camaraas con ID: {CamaraId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Camaraas no encontrado con ID: {CamaraId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {CamaraId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCamaraAsync(int id)
        {
            try
            {
                var deleted = await _CamaraBusiness.DeleteCamaraAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Camaraas no encontrado o ya eliminado" });

                return Ok(new { message = "Camaraas eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Camaraas con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
