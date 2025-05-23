// API/Controllers/RolesController.cs

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
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;
        private readonly ILogger<RolesController> _logger; // Inyectamos ILogger

        public RolesController(
            IRolService rolService,
            ILogger<RolesController> logger) // Inyectamos ILogger
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetAllRoles()
        {
            _logger.LogInformation("Inicio de GetAllRoles.");
            var rolDtos = await _rolService.GetAllRolesAsync();
            _logger.LogInformation("Se recuperaron {Count} roles.", rolDtos?.Count() ?? 0);
            return Ok(rolDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolDTO>> GetRolById(int id)
        {
            _logger.LogInformation("Inicio de GetRolById para ID: {Id}.", id);
            var rolDto = await _rolService.GetRolByIdAsync(id);
            if (rolDto == null)
            {
                _logger.LogWarning("Rol con ID {Id} no encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Rol con ID {Id} encontrado exitosamente.", id);
            return Ok(rolDto);
        }

        [HttpPost]
        public async Task<ActionResult<RolDTO>> CreateRol([FromBody] RolCreateUpdateDTO rolCreateDto) // Usamos el nuevo DTO
        {
            _logger.LogInformation("Inicio de CreateRol.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de rol inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            var createdRolDto = await _rolService.CreateRolAsync(rolCreateDto);
            _logger.LogInformation("Rol con ID {Id} creado exitosamente.", createdRolDto.Id);

            return CreatedAtAction(nameof(GetRolById), new { id = createdRolDto.Id }, createdRolDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolCreateUpdateDTO rolUpdateDto) // Usamos el nuevo DTO
        {
            _logger.LogInformation("Inicio de UpdateRol para ID: {Id}.", id);

            // La validación de que el ID del DTO coincida con el ID de la ruta se eliminó
            // ya que el ID de la ruta es la fuente de verdad principal para la actualización.
            // rolUpdateDto.Id = id; // Si necesitas pasar el ID al DTO para el servicio, AutoMapper puede manejar esto también

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de rol inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            var result = await _rolService.UpdateRolAsync(id, rolUpdateDto);
            if (!result)
            {
                _logger.LogWarning("Rol con ID {Id} no encontrado o no se pudo actualizar.", id);
                return NotFound();
            }
            _logger.LogInformation("Rol con ID {Id} actualizado exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            _logger.LogInformation("Inicio de DeleteRol (borrado lógico) para ID: {Id}.", id);
            var result = await _rolService.DeleteRolAsync(id);
            if (!result)
            {
                _logger.LogWarning("Rol con ID {Id} no encontrado o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Rol con ID {Id} eliminado lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}