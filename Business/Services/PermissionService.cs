using Business.Interfaces;
using Business.Validations;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IPermissionRepository permissionRepository, ILogger<PermissionService> logger)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _permissionRepository.GetAllAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            return await _permissionRepository.GetByIdAsync(id);
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            // You can add business logic before creating the permission
            LogicValidations.ValidatePermissionName(permission.name, _logger);
            return await _permissionRepository.AddAsync(permission);
        }

        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            var existingPermission = await _permissionRepository.GetByIdAsync(permission.id);
            LogicValidations.ValidateExistingPermission(existingPermission, permission.id, _logger);

            // Validaciones usando la clase LogicValidations
            LogicValidations.ValidatePermissionName(permission.name, _logger);
            // You can add business logic before updating the permission
            return await _permissionRepository.UpdateAsync(permission);
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            // You can add business logic before deleting the permission
            var existingPermission = await _permissionRepository.GetByIdAsync(id);
            LogicValidations.ValidateExistingPermission(existingPermission, id, _logger);
            return await _permissionRepository.DeleteAsync(id);
        }
    }
}