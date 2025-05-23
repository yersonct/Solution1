// API/Controllers/FormsController.cs

using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model; // Asegúrate de que 'Form' singular se resuelva correctamente
using Entity.DTOs; // Usar DTOs para el controlador
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; // Añadir para logging

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")] // Asegúrate de que tu política de autorización esté configurada
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ILogger<FormsController> _logger; // Inyectar ILogger

        public FormsController(IFormService formService, ILogger<FormsController> logger)
        {
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetAllForms()
        {
            _logger.LogInformation("Inicio de GetAllForms.");
            var formDtos = await _formService.GetAllFormsAsync();
            _logger.LogInformation("Se recuperaron {Count} formularios.", formDtos?.Count() ?? 0);
            return Ok(formDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetFormById(int id)
        {
            _logger.LogInformation("Inicio de GetFormById para ID: {Id}.", id);
            var formDto = await _formService.GetFormByIdAsync(id); // El servicio ya devuelve un DTO
            if (formDto == null)
            {
                _logger.LogWarning("Formulario con ID {Id} no encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Formulario con ID {Id} encontrado exitosamente.", id);
            return Ok(formDto);
        }

        [HttpPost]
        public async Task<ActionResult<FormDTO>> CreateForm([FromBody] FormCreateUpdateDTO formCreateDto)
        {
            _logger.LogInformation("Inicio de CreateForm.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de formulario inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            var createdFormDto = await _formService.CreateFormAsync(formCreateDto); // El servicio ya devuelve un DTO
            _logger.LogInformation("Formulario con ID {Id} creado exitosamente.", createdFormDto.Id);

            return CreatedAtAction(nameof(GetFormById), new { id = createdFormDto.Id }, createdFormDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] FormCreateUpdateDTO formUpdateDto)
        {
            _logger.LogInformation("Inicio de UpdateForm para ID: {Id}.", id);

            // Ya no necesitas verificar id != formDto.Id si no pasas el ID en el body del DTO de actualización
            // y confías en el ID de la ruta.
            // Si tu FormCreateUpdateDTO tuviera un Id, podrías mantener la validación,
            // pero es más común que el ID venga solo en la ruta para PUT.

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de formulario inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            var result = await _formService.UpdateFormAsync(id, formUpdateDto);
            if (!result)
            {
                _logger.LogWarning("Formulario con ID {Id} no encontrado o no se pudo actualizar.", id);
                return NotFound(); // O BadRequest si la actualización falló por otra razón de negocio
            }
            _logger.LogInformation("Formulario con ID {Id} actualizado exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            _logger.LogInformation("Inicio de DeleteForm (borrado lógico) para ID: {Id}.", id);
            var result = await _formService.DeleteFormAsync(id);
            if (!result)
            {
                _logger.LogWarning("Formulario con ID {Id} no encontrado o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Formulario con ID {Id} eliminado lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}