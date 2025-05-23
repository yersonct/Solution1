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
                FormId = formRolPermissionDto.FormId,
                RolId = formRolPermissionDto.RolId,
                PermissionId = formRolPermissionDto.PermissionId,
                Active = true // Set active on creation
                // You might need to fetch the related entities (Forms, Rol, Permission) here
                // if you want to ensure they exist before creating the FormRolPermission.
            };

            try
            {
                var createdFormRolPermission = await _formRolPermissionService.CreateFormRolPermissionAsync(formRolPermission);
                return CreatedAtAction(nameof(GetFormRolPermissionById), new { id = createdFormRolPermission.Id }, createdFormRolPermission);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error creating form role permission.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormRolPermission(int id, [FromBody] FormRolPermissionDTO formRolPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formRolPermissionDto.Id)
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
                Id = formRolPermissionDto.Id,
                Forms = new Forms { Name = formRolPermissionDto.FormName },
                Rol = new Rol { Name = formRolPermissionDto.RolName },
                Permission = new Permission { Name = formRolPermissionDto.PermissionName },
                Active = formRolPermissionDto.Active
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


}