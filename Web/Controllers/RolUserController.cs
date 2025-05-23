using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Linq; // Agregado para usar .Any() o .ToList() de forma más eficiente

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")]
    public class RolUsersController : ControllerBase
    {
        private readonly IRolUserService _rolUserService;
        private readonly ILogger<RolUsersController> _logger;

        public RolUsersController(
            IRolUserService rolUserService,
            ILogger<RolUsersController> logger)
        {
            _rolUserService = rolUserService ?? throw new ArgumentNullException(nameof(rolUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las relaciones Rol-Usuario.
        /// </summary>
        /// <returns>Una lista de RolUserDTO.</returns>
        [HttpGet]
        // Considera usar un IActionResult si el tipo de retorno puede variar (ej. NotFound, BadRequest)
        public async Task<ActionResult<IEnumerable<RolUserDTO>>> GetAllRolUsers()
        {
            _logger.LogInformation("Inicio de GetAllRolUsers.");
            try
            {
                var rolUsers = await _rolUserService.GetAllRolUsersAsync();

                // Optimización: Evitar Count() en IEnumerable si no es necesario.
                // Si el servicio devuelve una colección que podría ser grande, y solo quieres verificar si hay elementos,
                // .Any() es más eficiente que .Count() o .ToList().
                // Si siempre necesitas el conteo o el resultado como una lista, .ToList() antes del conteo.
                int count = rolUsers?.Count() ?? 0; // Usar Count() si la enumeración es barata o ya es una lista

                // Mejor registro para saber si se encontraron datos.
                if (count == 0)
                {
                    _logger.LogInformation("No se encontraron relaciones Rol-Usuario.");
                    // Podrías devolver Ok([]) o NotFound() dependiendo de la semántica deseada.
                    return Ok(Enumerable.Empty<RolUserDTO>()); // Devuelve un array vacío en lugar de null
                }

                _logger.LogInformation("Se recuperaron {Count} relaciones Rol-Usuario.", count);
                return Ok(rolUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todas las relaciones Rol-Usuario.");
                return StatusCode(500, "Error interno del servidor al recuperar relaciones Rol-Usuario.");
            }
        }

        /// <summary>
        /// Obtiene una relación Rol-Usuario por su ID.
        /// </summary>
        /// <param name="id">El ID de la relación Rol-Usuario.</param>
        /// <returns>La RolUserDTO correspondiente o NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RolUserDTO>> GetRolUserById(int id)
        {
            _logger.LogInformation("Inicio de GetRolUserById para ID: {Id}.", id);
            try
            {
                // Validación básica para evitar IDs negativos o cero si no son válidos en tu modelo.
                if (id <= 0)
                {
                    _logger.LogWarning("ID de relación Rol-Usuario inválido proporcionado: {Id}.", id);
                    return BadRequest("El ID de la relación Rol-Usuario debe ser un número positivo.");
                }

                var rolUser = await _rolUserService.GetRolUserByIdAsync(id);
                if (rolUser == null)
                {
                    _logger.LogWarning("Relación Rol-Usuario con ID {Id} no encontrada.", id);
                    return NotFound();
                }
                _logger.LogInformation("Relación Rol-Usuario con ID {Id} encontrada exitosamente.", id);
                return Ok(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar la relación Rol-Usuario con ID: {Id}.", id);
                return StatusCode(500, $"Error interno del servidor al recuperar la relación Rol-Usuario con ID {id}.");
            }
        }

        /// <summary>
        /// Crea una nueva relación Rol-Usuario.
        /// </summary>
        /// <param name="rolUserCreateDto">Los datos para crear la relación Rol-Usuario.</param>
        /// <returns>La RolUserDTO creada.</returns>
        [HttpPost]
        public async Task<ActionResult<RolUserDTO>> CreateRolUser([FromBody] RolUserCreateDTO rolUserCreateDto)
        {
            _logger.LogInformation("Inicio de CreateRolUser.");

            // ModelState.IsValid ya se maneja automáticamente por el atributo [ApiController]
            // pero si necesitas lógica personalizada para el BadRequest, puedes dejarlo.
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de Rol-Usuario inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var createdRolUserDto = await _rolUserService.CreateRolUserAsync(rolUserCreateDto);
                if (createdRolUserDto == null)
                {
                    // Esto puede ocurrir si el servicio tiene validaciones adicionales
                    _logger.LogWarning("La creación de la relación Rol-Usuario falló, posiblemente por datos duplicados o reglas de negocio.");
                    return Conflict("No se pudo crear la relación Rol-Usuario. Verifique los datos proporcionados.");
                }

                _logger.LogInformation("Relación Rol-Usuario con ID {Id} creada exitosamente.", createdRolUserDto.Id);
                return CreatedAtAction(nameof(GetRolUserById), new { id = createdRolUserDto.Id }, createdRolUserDto);
            }
            catch (ArgumentException aex)
            {
                // Capturar excepciones específicas del servicio, por ejemplo, si un ID no existe.
                _logger.LogWarning(aex, "Error de argumento al crear relación Rol-Usuario: {Message}", aex.Message);
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la relación Rol-Usuario.");
                return StatusCode(500, "Error interno del servidor al crear la relación Rol-Usuario.");
            }
        }

        /// <summary>
        /// Actualiza una relación Rol-Usuario existente.
        /// </summary>
        /// <param name="id">El ID de la relación Rol-Usuario a actualizar.</param>
        /// <param name="rolUserUpdateDto">Los datos para actualizar la relación Rol-Usuario.</param>
        /// <returns>NoContent si la actualización fue exitosa, o NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRolUser(int id, [FromBody] RolUserCreateDTO rolUserUpdateDto)
        {
            _logger.LogInformation("Inicio de UpdateRolUser para ID: {Id}.", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de relación Rol-Usuario inválido para actualización: {Id}.", id);
                return BadRequest("El ID de la relación Rol-Usuario debe ser un número positivo para la actualización.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de Rol-Usuario inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _rolUserService.UpdateRolUserAsync(id, rolUserUpdateDto);
                if (!result)
                {
                    _logger.LogWarning("Relación Rol-Usuario con ID {Id} no encontrada o no se pudo actualizar.", id);
                    return NotFound($"La relación Rol-Usuario con ID {id} no fue encontrada o no se pudo actualizar.");
                }
                _logger.LogInformation("Relación Rol-Usuario con ID {Id} actualizada exitosamente.", id);
                return NoContent();
            }
            catch (ArgumentException aex)
            {
                _logger.LogWarning(aex, "Error de argumento al actualizar relación Rol-Usuario con ID {Id}: {Message}", id, aex.Message);
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación Rol-Usuario con ID: {Id}.", id);
                return StatusCode(500, $"Error interno del servidor al actualizar la relación Rol-Usuario con ID {id}.");
            }
        }

        /// <summary>
        /// Elimina lógicamente una relación Rol-Usuario por su ID.
        /// </summary>
        /// <param name="id">El ID de la relación Rol-Usuario a eliminar.</param>
        /// <returns>NoContent si la eliminación fue exitosa, o NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolUser(int id)
        {
            _logger.LogInformation("Inicio de DeleteRolUser (borrado lógico) para ID: {Id}.", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de relación Rol-Usuario inválido para eliminación: {Id}.", id);
                return BadRequest("El ID de la relación Rol-Usuario debe ser un número positivo para la eliminación.");
            }

            try
            {
                var result = await _rolUserService.DeleteRolUserAsync(id); // Usa el método de borrado lógico del servicio
                if (!result)
                {
                    _logger.LogWarning("Relación Rol-Usuario con ID {Id} no encontrada o no se pudo eliminar lógicamente.", id);
                    return NotFound($"La relación Rol-Usuario con ID {id} no fue encontrada o no se pudo eliminar lógicamente.");
                }
                _logger.LogInformation("Relación Rol-Usuario con ID {Id} eliminada lógicamente exitosamente.", id);
                return NoContent(); // 204 No Content para una eliminación exitosa
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente la relación Rol-Usuario con ID: {Id}.", id);
                return StatusCode(500, $"Error interno del servidor al eliminar lógicamente la relación Rol-Usuario con ID {id}.");
            }
        }
    }
}