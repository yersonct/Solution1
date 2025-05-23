using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FormRolPermissionService : IFormRolPermissionService
    {
        private readonly IFormRolPermissionRepository _formRolPermissionRepository;
        private readonly ILogger<FormRolPermissionService> _logger;

        public FormRolPermissionService(IFormRolPermissionRepository formRolPermissionRepository, ILogger<FormRolPermissionService> logger)
        {
            _formRolPermissionRepository = formRolPermissionRepository ?? throw new ArgumentNullException(nameof(formRolPermissionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllFormRolPermissionsAsync()
        {
            try
            {
                var result = await _formRolPermissionRepository.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all form role permissions.");
                throw; // Re-throw the exception for the controller to handle
            }
        }

        public async Task<FormRolPermissionDTO> GetFormRolPermissionByIdAsync(int id)
        {
            try
            {
                var result = await _formRolPermissionRepository.GetByIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting form role permission by ID: {Id}", id);
                throw; // Re-throw the exception
            }
        }

        public async Task<FormRolPermission> CreateFormRolPermissionAsync(FormRolPermission entity)
        {
            try
            {
                var createDto = new FormRolPermissionCreateDTO
                {
                    FormId = entity.FormId,
                    RolId = entity.RolId,
                    PermissionId = entity.PermissionId
                };
                var createdDto = await _formRolPermissionRepository.AddAsync(createDto);

                return new FormRolPermission
                {
                    Id = createdDto.Id,
                    FormId = entity.FormId,
                    RolId = entity.RolId,
                    PermissionId = entity.PermissionId,
                    Forms = entity.Forms, // You might need to fetch these if not tracked
                    Rol = entity.Rol,     // depending on your context's change tracking
                    Permission = entity.Permission,
                    Active = createdDto.Active
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating form role permission.");
                throw; // Re-throw the exception
            }
        }

        public async Task<bool> UpdateFormRolPermissionAsync(FormRolPermission entity)
        {
            try
            {
                var updateDto = new FormRolPermissionDTO
                {
                    Id = entity.Id,
                    FormName = entity.Forms?.Name ?? "",
                    RolName = entity.Rol?.Name ?? "",
                    PermissionName = entity.Permission?.Name ?? "",
                    Active = entity.Active
                };
                var result = await _formRolPermissionRepository.UpdateAsync(updateDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating form role permission.");
                throw; // Re-throw the exception
            }
        }

        public async Task<bool> DeleteFormRolPermissionAsync(int id)
        {
            try
            {
                var result = await _formRolPermissionRepository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting form role permission with ID: {Id}", id);
                throw; // Re-throw the exception
            }
        }
    }
}