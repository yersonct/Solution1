using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Add this using statement
using System.Linq; // Add this using

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolUsersController : ControllerBase
    {
        private readonly IRolUserService _rolUserService;

        public RolUsersController(IRolUserService rolUserService)
        {
            _rolUserService = rolUserService ?? throw new ArgumentNullException(nameof(rolUserService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolUserDTO>>> GetAllRolUsers() // Change to RolUserDTO
        {
            var rolUsers = await _rolUserService.GetAllRolUsersAsync();
            var rolUserDtos = rolUsers.Select(ru => new RolUserDTO // Project to DTO
            {
                id = ru.id,
                id_rol = ru.id_rol,
                id_user = ru.id_user,
                RolName = ru.Rol?.Name, // Access Name property
                UserName = ru.User?.username // Access Name property
            }).ToList();
            return Ok(rolUserDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolUserDTO>> GetRolUserById(int id) // Change to RolUserDTO
        {
            var rolUser = await _rolUserService.GetRolUserByIdAsync(id);
            if (rolUser == null)
            {
                return NotFound();
            }

            var rolUserDto = new RolUserDTO // Project to DTO
            {
                id = rolUser.id,
                id_rol = rolUser.id_rol,
                id_user = rolUser.id_user,
                RolName = rolUser.Rol?.Name, // Access Name property
                UserName = rolUser.User?.username // Access Name property
            };
            return Ok(rolUserDto);
        }

        [HttpPost]
        public async Task<ActionResult<RolUser>> CreateRolUser([FromBody] RolUserCreateDTO rolUserCreateDto) // Use CreateDTO
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rolUser = new RolUser
            {
                id_rol = rolUserCreateDto.id_rol,
                id_user = rolUserCreateDto.id_user,
            };

            var createdRolUser = await _rolUserService.CreateRolUserAsync(rolUser);
            return CreatedAtAction(nameof(GetRolUserById), new { id = createdRolUser.id }, createdRolUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRolUser(int id, [FromBody] RolUserDTO rolUserDto) // Use RolUserDTO
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


            var result = await _rolUserService.UpdateRolUserAsync(existingRolUser);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolUser(int id)
        {
            var result = await _rolUserService.DeleteRolUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}