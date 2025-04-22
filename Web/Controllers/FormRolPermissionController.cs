using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;
using System.Linq;
using Entity.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormRolPermissionsController : ControllerBase
    {
        private readonly IFormRolPermissionService _formRolPermissionService;

        public FormRolPermissionsController(IFormRolPermissionService formRolPermissionService)
        {
            _formRolPermissionService = formRolPermissionService ?? throw new ArgumentNullException(nameof(formRolPermissionService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormRolPermissionDTO>>> GetAllFormRolPermissions()
        {
            var formRolPermissions = await _formRolPermissionService.GetAllFormRolPermissionsAsync();
            var formRolPermissionDtos = formRolPermissions.Select(frp => new FormRolPermissionDTO
            {
                id = frp.id,
                id_forms = frp.id_forms,
                id_rol = frp.id_rol,
                id_permission = frp.id_permission,
            }).ToList();
            return Ok(formRolPermissionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormRolPermissionDTO>> GetFormRolPermissionById(int id)
        {
            var formRolPermission = await _formRolPermissionService.GetFormRolPermissionByIdAsync(id);
            if (formRolPermission == null)
            {
                return NotFound();
            }

            var formRolPermissionDto = new FormRolPermissionDTO
            {
                id = formRolPermission.id,
                id_forms = formRolPermission.id_forms,
                id_rol = formRolPermission.id_rol,
                id_permission = formRolPermission.id_permission,
            };
            return Ok(formRolPermissionDto);
        }

        [HttpPost]
        public async Task<ActionResult<FormRolPermissionDTO>> CreateFormRolPermission([FromBody] FormRolPermissionCreateDTO formRolPermissionCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formRolPermission = new FormRolPermission
            {
                id_forms = formRolPermissionCreateDto.id_forms,
                id_rol = formRolPermissionCreateDto.id_rol,
                id_permission = formRolPermissionCreateDto.id_permission
            };

            var createdFormRolPermission = await _formRolPermissionService.CreateFormRolPermissionAsync(formRolPermission);

            var formRolPermissionDto = new FormRolPermissionDTO
            {
                id = createdFormRolPermission.id,
                id_forms = createdFormRolPermission.id_forms,
                id_rol = createdFormRolPermission.id_rol,
                id_permission = createdFormRolPermission.id_permission,
            };


            return CreatedAtAction(nameof(GetFormRolPermissionById), new { id = formRolPermissionDto.id }, formRolPermissionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormRolPermission(int id, [FromBody] FormRolPermissionDTO formRolPermissionDto)
        {
            if (id != formRolPermissionDto.id)
            {
                return BadRequest("El ID del FormRolPermission no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFormRolPermission = await _formRolPermissionService.GetFormRolPermissionByIdAsync(id);
            if (existingFormRolPermission == null)
            {
                return NotFound();
            }

            // Convertir el DTO a la entidad FormRolPermission
            var formRolPermissionToUpdate = new FormRolPermission
            {
                id = existingFormRolPermission.id, // Preservar el ID
                id_forms = formRolPermissionDto.id_forms,
                id_rol = formRolPermissionDto.id_rol,
                id_permission = formRolPermissionDto.id_permission,
                // No actualizar Forms, RolName, PermissionName aquí. Estos son de solo lectura para las actualizaciones.
            };


            var result = await _formRolPermissionService.UpdateFormRolPermissionAsync(formRolPermissionToUpdate);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormRolPermission(int id)
        {
            var result = await _formRolPermissionService.DeleteFormRolPermissionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}