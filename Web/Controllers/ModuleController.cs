// API/Controllers/ModulesController.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Importar para logging
using Business.Interfaces;
using Entity.DTOs; // Usar DTOs para el controlador
using Microsoft.AspNetCore.Authorization;
using Entity.Model; // Asegurarse de que Module (singular) se resuelva correctamente

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly ILogger<ModulesController> _logger; // Inyectamos ILogger

        public ModulesController(
            IModuleService moduleService,
            ILogger<ModulesController> logger) // Inyectamos ILogger
        {
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDTO>>> GetAllModules()
        {
            _logger.LogInformation("Inicio de GetAllModules.");
            var moduleDtos = await _moduleService.GetAllModulesAsync();
            _logger.LogInformation("Se recuperaron {Count} módulos.", moduleDtos?.Count() ?? 0);
            return Ok(moduleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModuleById(int id)
        {
            _logger.LogInformation("Inicio de GetModuleById para ID: {Id}.", id);
            var moduleDto = await _moduleService.GetModuleByIdAsync(id);
            if (moduleDto == null)
            {
                _logger.LogWarning("Módulo con ID {Id} no encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Módulo con ID {Id} encontrado exitosamente.", id);
            return Ok(moduleDto);
        }

        [HttpPost]
        public async Task<ActionResult<ModuleDTO>> CreateModule([FromBody] ModuleCreateUpdateDTO moduleCreateDto) // Usamos el nuevo DTO
        {
            _logger.LogInformation("Inicio de CreateModule.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de módulo inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            var createdModuleDto = await _moduleService.CreateModuleAsync(moduleCreateDto);
            _logger.LogInformation("Módulo con ID {Id} creado exitosamente.", createdModuleDto.Id);

            return CreatedAtAction(nameof(GetModuleById), new { id = createdModuleDto.Id }, createdModuleDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] ModuleCreateUpdateDTO moduleUpdateDto) // Usamos el nuevo DTO
        {
            _logger.LogInformation("Inicio de UpdateModule para ID: {Id}.", id);

            // La validación de que el ID del DTO coincida con el ID de la ruta se eliminó
            // ya que el ID de la ruta es la fuente de verdad principal para la actualización.
            // moduleUpdateDto.Id = id; // Si necesitas pasar el ID al DTO para el servicio, AutoMapper puede manejar esto también

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de módulo inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            var result = await _moduleService.UpdateModuleAsync(id, moduleUpdateDto);
            if (!result)
            {
                _logger.LogWarning("Módulo con ID {Id} no encontrado o no se pudo actualizar.", id);
                return NotFound();
            }
            _logger.LogInformation("Módulo con ID {Id} actualizado exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            _logger.LogInformation("Inicio de DeleteModule (borrado lógico) para ID: {Id}.", id);
            var result = await _moduleService.DeleteModuleAsync(id);
            if (!result)
            {
                _logger.LogWarning("Módulo con ID {Id} no encontrado o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Módulo con ID {Id} eliminado lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}