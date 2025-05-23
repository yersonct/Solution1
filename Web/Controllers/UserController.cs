// API/Controllers/UsersController.cs

using System;
using System.Collections.Generic;
using System.Linq; // Mantener si usas LINQ para algo más, aunque aquí ya no es crítico
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // IPersonService fue eliminada de aquí, su uso se delega a UserService
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            _logger.LogInformation("Inicio de GetAllUsers.");
            // El servicio ahora devuelve los DTOs ya completos con PersonName
            var userDtos = await _userService.GetAllUsersWithPersonNameAsync();
            _logger.LogInformation("Se recuperaron {Count} usuarios.", userDtos?.Count() ?? 0);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            _logger.LogInformation("Inicio de GetUserById para ID: {Id}.", id);
            // El servicio ahora devuelve el DTO ya completo con PersonName
            var userDto = await _userService.GetUserWithPersonNameByIdAsync(id);
            if (userDto == null)
            {
                _logger.LogWarning("Usuario con ID {Id} no encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Usuario con ID {Id} encontrado exitosamente.", id);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            _logger.LogInformation("Inicio de CreateUser.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de creación de usuario inválida. Errores: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            // El servicio recibe el DTO y devuelve el DTO creado (con ID y PersonName)
            var createdUserDto = await _userService.CreateUserAsync(userCreateDTO);
            _logger.LogInformation("Usuario con ID {Id} creado exitosamente.", createdUserDto.Id);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.Id }, createdUserDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDTO) // Usamos UserUpdateDTO
        {
            _logger.LogInformation("Inicio de UpdateUser para ID: {Id}.", id);

            // Si decides validar que el ID en el DTO (si lo incluyes) coincida con el ID de la ruta
            // if (userUpdateDTO.Id != id) {
            //     _logger.LogWarning("ID de ruta ({RouteId}) no coincide con ID de DTO ({DtoId}) para actualización.", id, userUpdateDTO.Id);
            //     return BadRequest("El ID del usuario en la ruta no coincide con el ID del usuario en el cuerpo de la petición.");
            // }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud de actualización de usuario inválida para ID {Id}. Errores: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            // El servicio se encarga de buscar el usuario y actualizarlo
            var result = await _userService.UpdateUserAsync(id, userUpdateDTO);
            if (!result)
            {
                _logger.LogWarning("Usuario con ID {Id} no encontrado o no se pudo actualizar.", id);
                return NotFound();
            }
            _logger.LogInformation("Usuario con ID {Id} actualizado exitosamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Inicio de DeleteUser (borrado lógico) para ID: {Id}.", id);
            // El servicio se encarga del borrado lógico
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                _logger.LogWarning("Usuario con ID {Id} no encontrado o no se pudo eliminar lógicamente.", id);
                return NotFound();
            }
            _logger.LogInformation("Usuario con ID {Id} eliminado lógicamente exitosamente.", id);
            return NoContent();
        }
    }
}