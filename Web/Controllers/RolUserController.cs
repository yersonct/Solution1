using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;
using System.Linq;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolUsersController : ControllerBase
    {
        private readonly IRolUserService _rolUserService;
        private readonly IRolService _rolService;
        private readonly IUserService _userService;

        public RolUsersController(
            IRolUserService rolUserService,
            IRolService rolService,
            IUserService userService)
        {
            _rolUserService = rolUserService ?? throw new ArgumentNullException(nameof(rolUserService));
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRolUsers()
        {
            try
            {
                var rolUsers = await _rolUserService.GetAllRolUsersAsync();
                var results = rolUsers
                    .Select(ru => new
                    {
                        RolName = ru.Rol?.Name,
                        UserName = ru.User?.username,
                        Active = ru.active
                    })
                    .ToList();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las relaciones Rol-Usuario: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRolUserById(int id)
        {
            try
            {
                var rolUser = await _rolUserService.GetRolUserByIdAsync(id);
                if (rolUser == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    RolName = rolUser.Rol?.Name,
                    UserName = rolUser.User?.username,
                    Active = rolUser.active
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la relación Rol-Usuario: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateRolUser([FromBody] RolUserCreateDTO rolUserCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rolUser = new RolUser
            {
                id_rol = rolUserCreateDto.id_rol,
                id_user = rolUserCreateDto.id_user,
                active = true
            };

            try
            {
                var createdRolUser = await _rolUserService.CreateRolUserAsync(rolUser);

                var rol = await _rolService.GetRolByIdAsync(createdRolUser.id_rol);
                var user = await _userService.GetUserByIdAsync(createdRolUser.id_user);

                return CreatedAtAction(nameof(GetRolUserById), new { id = createdRolUser.id }, new
                {
                    RolName = rol?.Name,
                    UserName = user?.username,
                    Active = createdRolUser.active
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la relación Rol-Usuario: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRolUser(int id, [FromBody] RolUserDTO rolUserDto)
        {
            if (id != rolUserDto.id)
            {
                return BadRequest("El ID de la relación Rol-Usuario no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRolUser = await _rolUserService.GetRolUserByIdAsync(id);
            if (existingRolUser == null)
            {
                return NotFound();
            }

            existingRolUser.id_rol = rolUserDto.id_rol;
            existingRolUser.id_user = rolUserDto.id_user;
            existingRolUser.active = rolUserDto.active; // Allow updating the active status

            try
            {
                var result = await _rolUserService.UpdateRolUserAsync(existingRolUser);
                if (!result)
                {
                    return StatusCode(500, "Error al actualizar la relación Rol-Usuario.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la relación Rol-Usuario: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolUser(int id)
        {
            try
            {
                var rolUserToDelete = await _rolUserService.GetRolUserByIdAsync(id);
                if (rolUserToDelete == null)
                {
                    return NotFound();
                }

                var result = await _rolUserService.DeleteRolUserAsync(id); // Use the logical delete method in the service
                if (!result)
                {
                    return StatusCode(500, "Error al eliminar lógicamente la relación Rol-Usuario.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar lógicamente la relación Rol-Usuario: {ex.Message}");
            }
        }
    }
}