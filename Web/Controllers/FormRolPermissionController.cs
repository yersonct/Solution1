using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthenticatedWithAny")]
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
            try
            {
                var formRolPermissions = await _formRolPermissionService.GetAllFormRolPermissionsAsync();
                return Ok(formRolPermissions);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving form role permissions.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormRolPermissionDTO>> GetFormRolPermissionById(int id)
        {
            try
            {
                var formRolPermission = await _formRolPermissionService.GetFormRolPermissionByIdAsync(id);
                if (formRolPermission == null)
                {
                    return NotFound();
                }
                return Ok(formRolPermission);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving form role permission.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FormRolPermission>> CreateFormRolPermission([FromBody] FormRolPermissionCreateDTO formRolPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formRolPermission = new FormRolPermission
            {
                id_forms = formRolPermissionDto.id_forms,
                id_rol = formRolPermissionDto.id_rol,
                id_permission = formRolPermissionDto.id_permission,
                active = true // Set active on creation
                // You might need to fetch the related entities (Forms, Rol, Permission) here
                // if you want to ensure they exist before creating the FormRolPermission.
            };

            try
            {
                var createdFormRolPermission = await _formRolPermissionService.CreateFormRolPermissionAsync(formRolPermission);
                return CreatedAtAction(nameof(GetFormRolPermissionById), new { id = createdFormRolPermission.id }, createdFormRolPermission);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error creating form role permission.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormRolPermission(int id, [FromBody] FormRolPermissionUpdateDTO formRolPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formRolPermissionDto.id)
            {
                return BadRequest("The ID in the request body does not match the route ID.");
            }

            var existingFormRolPermission = await _formRolPermissionService.GetFormRolPermissionByIdAsync(id);
            if (existingFormRolPermission == null)
            {
                return NotFound();
            }

            var formRolPermissionToUpdate = new FormRolPermission
            {
                id = formRolPermissionDto.id,
                Forms = new Forms { name = formRolPermissionDto.formName },
                Rol = new Rol { name = formRolPermissionDto.rolName },
                Permission = new Permission { name = formRolPermissionDto.permissionName },
                active = formRolPermissionDto.active
            };

            try
            {
                var result = await _formRolPermissionService.UpdateFormRolPermissionAsync(formRolPermissionToUpdate);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating form role permission.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormRolPermission(int id)
        {
            try
            {
                var result = await _formRolPermissionService.DeleteFormRolPermissionAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting form role permission.");
            }
        }
    }

    public class FormRolPermissionUpdateDTO
    {
        public int id { get; set; }
        public string formName { get; set; }
        public string rolName { get; set; }
        public string permissionName { get; set; }
        public bool active { get; set; }
    }
}