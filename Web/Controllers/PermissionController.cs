using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Asegúrate de tener el namespace de tus DTOs
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            var permissionDtos = permissions.Select(p => new PermissionDTO
            {
                id = p.id,
                name = p.name,
            }).ToList();
            return Ok(permissionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            var permissionDto = new PermissionDTO
            {
                id = permission.id,
                name = permission.name,
            };
            return Ok(permissionDto);
        }

        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission([FromBody] PermissionDTO permissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permission = new Permission
            {
                name = permissionDto.name,
            };

            var createdPermission = await _permissionService.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.id }, createdPermission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDTO permissionDto)
        {
            if (id != permissionDto.id)
            {
                return BadRequest("El ID del permiso no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPermission = await _permissionService.GetPermissionByIdAsync(id);
            if (existingPermission == null)
            {
                return NotFound();
            }

            existingPermission.name = permissionDto.name;


            var result = await _permissionService.UpdatePermissionAsync(existingPermission);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
