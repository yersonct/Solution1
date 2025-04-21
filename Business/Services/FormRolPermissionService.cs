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
            return await _formRolPermissionRepository.GetAllAsync();
        }

        public async Task<FormRolPermissionDTO?> GetFormRolPermissionByIdAsync(int id)
        {
            return await _formRolPermissionRepository.GetByIdAsync(id);
        }

        public async Task<FormRolPermissionDTO> CreateFormRolPermissionAsync(FormRolPermissionCreateDTO formRolPermission)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el permiso de rol para el formulario
            return await _formRolPermissionRepository.AddAsync(formRolPermission);
        }

        public async Task<bool> UpdateFormRolPermissionAsync(FormRolPermissionDTO formRolPermission)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el permiso de rol para el formulario
            return await _formRolPermissionRepository.UpdateAsync(formRolPermission);
        }

        public async Task<bool> DeleteFormRolPermissionAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el permiso de rol para el formulario
            return await _formRolPermissionRepository.DeleteAsync(id);
        }

        Task<IEnumerable<FormRolPermissionDTO>> IFormRolPermissionService.GetAllFormRolPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        Task<FormRolPermissionDTO?> IFormRolPermissionService.GetFormRolPermissionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<FormRolPermissionDTO> IFormRolPermissionService.CreateFormRolPermissionAsync(FormRolPermission formRolPermission)
        {
            throw new NotImplementedException();
        }

        Task<bool> IFormRolPermissionService.UpdateFormRolPermissionAsync(FormRolPermissionDTO formRolPermission)
        {
            throw new NotImplementedException();
        }

        Task<bool> IFormRolPermissionService.DeleteFormRolPermissionAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
