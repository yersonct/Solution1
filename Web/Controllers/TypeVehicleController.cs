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
    public class TypeVehicleController : ControllerBase
    {
        private readonly TypeVehicleBusiness _typeVehicleBusiness;
        private readonly ILogger<TypeVehicleController> _logger;

        public TypeVehicleController(TypeVehicleBusiness typeVehicleBusiness, ILogger<TypeVehicleController> logger)
        {
            _typeVehicleBusiness = typeVehicleBusiness ?? throw new ArgumentNullException(nameof(typeVehicleBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TypeVehicleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllTypeVehicles()
        {
            try
            {
                var typeVehicles = await _typeVehicleBusiness.GetAllTypeVehiclesAsync();
                return Ok(typeVehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los TypeVehicles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TypeVehicleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTypeVehicleById(int id)
        {
            try
            {
                var typeVehicle = await _typeVehicleBusiness.GetTypeVehicleByIdAsync(id);
                return Ok(typeVehicle);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para TypeVehicle con ID: {TypeVehicleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "TypeVehicle no encontrado con ID: {TypeVehicleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener TypeVehicle con ID: {TypeVehicleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TypeVehicleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTypeVehicleAsync([FromBody] TypeVehicleDTO typeVehicleDTO)
        {
            try
            {
                var createdTypeVehicle = await _typeVehicleBusiness.CreateTypeVehicleAsync(typeVehicleDTO);
                return CreatedAtAction(nameof(GetTypeVehicleById), new { id = createdTypeVehicle.id }, createdTypeVehicle);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el TypeVehicle");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el TypeVehicle");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TypeVehicleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTypeVehicleAsync(int id, [FromBody] TypeVehicleDTO typeVehicleDTO)
        {
            if (id != typeVehicleDTO.id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {
                var updatedTypeVehicle = await _typeVehicleBusiness.UpdateTypeVehicleAsync(typeVehicleDTO);
                return Ok(updatedTypeVehicle);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el TypeVehicle con ID: {TypeVehicleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "TypeVehicle no encontrado con ID: {TypeVehicleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el TypeVehicle con ID: {TypeVehicleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTypeVehicleAsync(int id)
        {
            try
            {
                var deleted = await _typeVehicleBusiness.DeleteTypeVehicleAsync(id);
                if (!deleted)
                    return NotFound(new { message = "TypeVehicle no encontrado o ya eliminado" });

                return Ok(new { message = "TypeVehicle eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el TypeVehicle con ID: {TypeVehicleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
