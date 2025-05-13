using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces; // Asegúrate de que esta interfaz exista
using Entity.DTOs; // Asegúrate de que estos DTOs existan
using Microsoft.Extensions.Logging;
using Entity.Model;
using Microsoft.AspNetCore.Authorization; // Asegúrate de que este namespace exista

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormModulesController : ControllerBase
    {
        private readonly IFormModuleService _formModuleService;
        private readonly ILogger<FormModulesController> _logger;

        public FormModulesController(IFormModuleService formModuleService, ILogger<FormModulesController> logger)
        {
            _formModuleService = formModuleService ?? throw new ArgumentNullException(nameof(formModuleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormModuleDTO>>> GetAllFormModules()
        {
            try
            {
                var formModules = await _formModuleService.GetAllFormModulesAsync();
                return Ok(formModules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormModules activos.");
                return StatusCode(500, "Internal Server Error"); // Mejor manejo de errores
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormModuleDTO>> GetFormModuleById(int id)
        {
            try
            {
                var formModule = await _formModuleService.GetFormModuleByIdAsync(id);
                if (formModule == null)
                {
                    _logger.LogWarning($"FormModule activo con ID {id} no encontrado");
                    return NotFound();
                }
                return Ok(formModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener FormModule activo con ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FormModuleDTO>> CreateFormModule([FromBody] FormModuleCreateDTO formModule)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido al crear FormModule");
                return BadRequest(ModelState);
            }

            try
            {
                var createdFormModule = await _formModuleService.CreateFormModuleAsync(formModule);
                return CreatedAtAction(nameof(GetFormModuleById), new { id = createdFormModule.Id }, createdFormModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear FormModule");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormModule(int id, [FromBody] FormModuleCreateDTO formModule)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Modelo inválido al actualizar FormModule con ID {id}");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _formModuleService.UpdateFormModuleAsync(id, formModule);
                if (!result)
                {
                    _logger.LogWarning($"FormModule activo con ID {id} no encontrado para actualizar");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar FormModule con ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormModule(int id)
        {
            try
            {
                var result = await _formModuleService.DeleteFormModuleAsync(id);
                if (!result)
                {
                    _logger.LogWarning($"FormModule activo con ID {id} no encontrado para eliminar");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar FormModule con ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}