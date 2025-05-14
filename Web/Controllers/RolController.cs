using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Asegúrate de tener el namespace de tus DTOs
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetAllRoles()
        {
            var roles = await _rolService.GetAllRolesAsync();
            var rolDtos = roles.Select(r => new RolDTO
            {
                id = r.id,
                Name = r.Name,
                Description = r.Description,
                Active = r.Active
            }).ToList();
            return Ok(rolDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolDTO>> GetRolById(int id)
        {
            var rol = await _rolService.GetRolByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            var rolDto = new RolDTO
            {
                id = rol.id,
                Name = rol.Name,
                Description = rol.Description,
                Active = rol.Active
            };
            return Ok(rolDto);
        }

        [HttpPost]
        public async Task<ActionResult<Rol>> CreateRol([FromBody] RolDTO rolDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rol = new Rol
            {
                Name = rolDto.Name,
                Description = rolDto.Description,
                Active = true // Asegúrate de que se cree como activo
            };

            var createdRol = await _rolService.CreateRolAsync(rol);
            return CreatedAtAction(nameof(GetRolById), new { id = createdRol.id }, createdRol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolDTO rolDto)
        {
            if (id != rolDto.id)
            {
                return BadRequest("El ID del rol no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRol = await _rolService.GetRolByIdAsync(id);
            if (existingRol == null)
            {
                return NotFound();
            }

            existingRol.Name = rolDto.Name;
            existingRol.Description = rolDto.Description;
            existingRol.Active = rolDto.Active; // Permite actualizar el estado activo

            var result = await _rolService.UpdateRolAsync(existingRol);
            if (!result)
            {
                return StatusCode(500, "Ocurrió un error al actualizar el rol.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var result = await _rolService.DeleteRolAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}