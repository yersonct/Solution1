// API/Controllers/PersonsController.cs

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
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PersonsController> _logger; // Inyectamos ILogger

        public PersonsController(
            IPersonService personService,
            ILogger<PersonsController> logger) // Inyectamos ILogger
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons()
        {
            _logger.LogInformation("Inicio de GetAllPersons.");
            var personDtos = await _personService.GetAllPersonsAsync(); // El servicio devuelve directamente los DTOs
            _logger.LogInformation("Se recuperaron {Count} personas.", personDtos?.Count() ?? 0);
            return Ok(personDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPersonById(int id)
        {
            _logger.LogInformation("Inicio de GetPersonById para ID: {Id}.", id);
            var personDto = await _personService.GetPersonByIdAsync(id); // El servicio devuelve directamente el DTO
            if (personDto == null)
            {
                _logger.LogWarning("Persona con ID {Id} no encontrada.", id);
                return NotFound();
            }
            _logger.LogInformation("Persona con ID {Id} encontrada exitosamente.", id);
            return Ok(personDto);
        }

        [HttpPost]
        public async Task<ActionResult<PersonDTO>> CreatePerson([FromBody] PersonCreateUpdateDTO createPersonDTO) // Usamos PersonCreateUpdateDTO
        {
            _logger.LogInformation("Inicio de CreatePerson.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de persona inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            var createdPersonDto = await _personService.CreatePersonAsync(createPersonDTO); // El servicio recibe y devuelve DTOs
            _logger.LogInformation("Persona con ID {Id} creada exitosamente.", createdPersonDto.Id);

            return CreatedAtAction(nameof(GetPersonById), new { id = createdPersonDto.Id }, createdPersonDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonCreateUpdateDTO updatePersonDTO) // Usamos PersonCreateUpdateDTO
        {
            _logger.LogInformation("Inicio de UpdatePerson para ID: {Id}.", id);

            // La validación de que el ID del DTO coincida con el ID de la ruta se eliminó
            // ya que el ID de la ruta es la fuente de verdad principal para la actualización.
            // Si el ID del DTO es necesario para la validación interna del DTO, asegúrate de ello.

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de persona inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            var result = await _personService.UpdatePersonAsync(id, updatePersonDTO); // El servicio se encarga de la lógica
            if (!result)
            {
                _logger.LogWarning("Persona con ID {Id} no encontrada o no se pudo actualizar.", id);
                return NotFound();
            }
            _logger.LogInformation("Persona con ID {Id} actualizada exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            _logger.LogInformation("Inicio de DeletePerson (borrado lógico) para ID: {Id}.", id);
            var result = await _personService.DeletePersonAsync(id); // El servicio se encarga del borrado lógico
            if (!result)
            {
                _logger.LogWarning("Persona con ID {Id} no encontrada o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Persona con ID {Id} eliminada lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}