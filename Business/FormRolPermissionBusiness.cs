using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class FormRolPermissionBusiness
    {
        private readonly FormRolPermissionData _data;
        private readonly ILogger<FormRolPermissionBusiness> _logger;

        public FormRolPermissionBusiness(FormRolPermissionData data, ILogger<FormRolPermissionBusiness> logger)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllAsync()
        {
            try
            {
                return await _data.GetAllAsyncSQL();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos FormRol.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos FormRol.", ex);
            }
        }

        public async Task<FormRolPermissionDTO> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            try
            {
                var item = await _data.GetByIdAsyncSQL(id);
                if (item == null)
                    throw new EntityNotFoundException("FormRolPermission", id);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener FormRolPermission con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el FormRolPermission con ID {id}.", ex);
            }
        }

        public async Task<FormRolPermissionDTO> CreateAsync(FormRolPermissionCreateDTO dto)
        {
            try
            {
                ValidateCreate(dto);

                // Validación de existencia en las tablas relacionadas
                if (!await _data.ExistsByIdAsync("role", dto.id_role))
                    throw new ValidationException("El rol especificado no existe.");

                if (!await _data.ExistsByIdAsync("forms", dto.id_forms))
                    throw new ValidationException("El formulario especificado no existe.");

                if (!await _data.ExistsByIdAsync("permissions", dto.id_permission))
                    throw new ValidationException("El permiso especificado no existe.");

                return await _data.CreateAsyncSQL(dto);
            }
            catch (ValidationException)
            {
                throw; // No loguear validaciones, se manejan en el controlador si es necesario
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear FormRolPermission.");
                throw new ExternalServiceException("Base de datos", "Error al crear el FormRolPermission.", ex);
            }
        }


        public async Task<bool> UpdateAsync(FormRolPermissionDTO dto)
        {
            try
            {
                ValidateUpdate(dto);

                var existing = await _data.GetByIdAsyncSQL(dto.Id);
                if (existing == null)
                    throw new EntityNotFoundException("FormRolPermission", dto.Id);

                return await _data.UpdateAsyncSQL(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar FormRolPermission con ID: {Id}", dto.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el FormRolPermission.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _data.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("FormRolPermission", id);

                return await _data.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar FormRolPermission con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el FormRolPermission.", ex);
            }
        }

        private void ValidateCreate(FormRolPermissionCreateDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto FormRolPermission no puede ser nulo.");

            if (dto.id_forms <= 0)
                throw new ValidationException("FormsId", "El campo FormsId es obligatorio.");

            if (dto.id_role <= 0)
                throw new ValidationException("RoleId", "El campo RoleId es obligatorio.");

            if (dto.id_permission <= 0)
                throw new ValidationException("PermissionsId", "El campo PermissionsId es obligatorio.");
        }

        private void ValidateUpdate(FormRolPermissionDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto FormRolPermission no puede ser nulo.");

            if (dto.Id <= 0)
                throw new ValidationException("Id", "El ID debe ser válido.");

            if (dto.FormId <= 0)
                throw new ValidationException("FormId", "El campo FormId es obligatorio.");

            if (dto.RolId <= 0)
                throw new ValidationException("RolId", "El campo RolId es obligatorio.");

            if (dto.PermissionId <= 0)
                throw new ValidationException("PermissionId", "El campo PermissionId es obligatorio.");
        }
    }
}
