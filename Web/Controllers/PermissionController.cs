// API/Controllers/PermissionsController.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Importar para logging
using Business.Interfaces;
using Entity.DTOs; // Usar DTOs para el controlador
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger; // Inyectamos ILogger

        public PermissionsController(
            IPermissionService permissionService,
            ILogger<PermissionsController> logger) // Inyectamos ILogger
        {
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetAllPermissions()
        {
            _logger.LogInformation("Inicio de GetAllPermissions.");
            var permissionDtos = await _permissionService.GetAllPermissionsAsync(); // El servicio devuelve directamente los DTOs
            _logger.LogInformation("Se recuperaron {Count} permisos.", permissionDtos?.Count() ?? 0);
            return Ok(permissionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermissionById(int id)
        {
            _logger.LogInformation("Inicio de GetPermissionById para ID: {Id}.", id);
            var permissionDto = await _permissionService.GetPermissionByIdAsync(id); // El servicio devuelve directamente el DTO
            if (permissionDto == null)
            {
                _logger.LogWarning("Permiso con ID {Id} no encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Permiso con ID {Id} encontrado exitosamente.", id);
            return Ok(permissionDto);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionDTO>> CreatePermission([FromBody] PermissionCreateUpdateDTO permissionCreateDto) // Usamos PermissionCreateUpdateDTO
        {
            _logger.LogInformation("Inicio de CreatePermission.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de permiso inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            var createdPermissionDto = await _permissionService.CreatePermissionAsync(permissionCreateDto); // El servicio recibe y devuelve DTOs
            _logger.LogInformation("Permiso con ID {Id} creado exitosamente.", createdPermissionDto.Id);

            return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermissionDto.Id }, createdPermissionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionCreateUpdateDTO permissionUpdateDto) // Usamos PermissionCreateUpdateDTO
        {
            _logger.LogInformation("Inicio de UpdatePermission para ID: {Id}.", id);

            // La validación de que el ID del DTO coincida con el ID de la ruta se eliminó
            // ya que el ID de la ruta es la fuente de verdad principal para la actualización.

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de permiso inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            var result = await _permissionService.UpdatePermissionAsync(id, permissionUpdateDto); // El servicio se encarga de la lógica
            if (!result)
            {
                _logger.LogWarning("Permiso con ID {Id} no encontrado o no se pudo actualizar.", id);
                return NotFound();
            }
            _logger.LogInformation("Permiso con ID {Id} actualizado exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            _logger.LogInformation("Inicio de DeletePermission (borrado lógico) para ID: {Id}.", id);
            var result = await _permissionService.DeletePermissionAsync(id); // El servicio se encarga del borrado lógico
            if (!result)
            {
                _logger.LogWarning("Permiso con ID {Id} no encontrado o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Permiso con ID {Id} eliminado lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}