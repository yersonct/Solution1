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
                FormsName = frp.FormsName,
                id_rol = frp.id_rol, // Keep this as the property name, ensure it matches your Entity
                RolName = frp.RolName,
                id_permission = frp.id_permission,
                PermissionName = frp.PermissionName
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
                FormsName = formRolPermission.FormsName,
                id_rol = formRolPermission.id_rol, // Keep this as the property name
                RolName = formRolPermission.RolName,
                id_permission = formRolPermission.id_permission,
                PermissionName = formRolPermission.PermissionName
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
                id_rol = formRolPermissionCreateDto.id_rol, // Keep this as the property name
                id_permission = formRolPermissionCreateDto.id_permission
            };

            var createdFormRolPermission = await _formRolPermissionService.CreateFormRolPermissionAsync(formRolPermission);

            var formRolPermissionDto = new FormRolPermissionDTO
            {
                id = createdFormRolPermission.id,
                id_forms = createdFormRolPermission.id_forms,
                FormsName = createdFormRolPermission.FormName,
                id_rol = createdFormRolPermission.id_rol, // Keep this as the property name
                RolName = createdFormRolPermission.RolName,
                id_permission = createdFormRolPermission.id_permission,
                PermissionName = createdFormRolPermission.PermissionName
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

            var existingFormRolPermissionDto = await _formRolPermissionService.GetFormRolPermissionByIdAsync(id);
            if (existingFormRolPermissionDto == null)
            {
                return NotFound();
            }

            // Convertir el DTO a la entidad FormRolPermission
            var existingFormRolPermission = new FormRolPermission
            {
                id = existingFormRolPermissionDto.id,
                id_forms = formRolPermissionDto.id_forms,
                id_rol = formRolPermissionDto.id_rol, // Keep this as the property name
                id_permission = formRolPermissionDto.id_permission,
                // Asegúrate de mapear all properties
            };


            var result = await _formRolPermissionService.UpdateFormRolPermissionAsync(existingFormRolPermission);
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
