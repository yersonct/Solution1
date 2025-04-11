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
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _PermissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(PermissionBusiness PermissionBusiness, ILogger<PermissionController> logger)
        {
            _PermissionBusiness = PermissionBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var Permissions = await _PermissionBusiness.GetAllPermissionsAsync();
                return Ok(Permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Permissionas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var Permission = await _PermissionBusiness.GetPermissionByIdAsync(id);
                return Ok(Permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para usuario con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Usuario no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermissionAsync([FromBody] PermissionDTO PermissionDTO)
        {
            try
            {
                var createdPermission = await _PermissionBusiness.CreatePermissionAsync(PermissionDTO);
                return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el Permissionas");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Permissionas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermissionAsync(int id, [FromBody] PermissionDTO PermissionDTO)
        {
            if (id != PermissionDTO.Id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {


                var updatedPermission = await _PermissionBusiness.UpdatePermissionAsync(PermissionDTO);
                return Ok(updatedPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el Permissionas con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permissionas no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePermissionAsync(int id)
        {
            try
            {
                var deleted = await _PermissionBusiness.DeletePermissionAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Permissionas no encontrado o ya eliminado" });

                return Ok(new { message = "Permissionas eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Permissionas con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
