using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PermissionBusiness
    {
        private readonly PermissionData _PermissionData;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(PermissionData PermissionData, ILogger<PermissionBusiness> logger)
        {
            _PermissionData = PermissionData ?? throw new ArgumentNullException(nameof(PermissionData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            try
            {
                var Permission = await _PermissionData.GetAllAsyncSQL();

                return MapToDtoList(Permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Permissionas.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Permissiona.", ex);
            }
        }

        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero.");
            }

            try
            {
                var Permission = await _PermissionData.GetByIdAsyncSQL(id);
                if (Permission == null)
                {
                    throw new EntityNotFoundException("Permission", id);
                }
                return MapToDTO(Permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}.", ex);
            }
        }

        public async Task<PermissionDTO> CreatePermissionAsync(PermissionDTO PermissionDTO)
        {
            try
            {
                ValidatePermission(PermissionDTO);
                var Permissiona = MapToEntity(PermissionDTO);
                var createdPermission = await _PermissionData.CreateAsyncSQL(Permissiona);
                return MapToDTO(createdPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario: {Permissionname}", PermissionDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> UpdatePermissionAsync(PermissionDTO PermissionDTO)
        {
            try
            {
                ValidatePermission(PermissionDTO);
                var existingPermission = await _PermissionData.GetByIdAsyncSQL(PermissionDTO.Id);
                if (existingPermission == null)
                {
                    throw new EntityNotFoundException("Permission", PermissionDTO.Id);
                }

                existingPermission.Name = PermissionDTO.Name;
        

                return await _PermissionData.UpdateAsyncSQL(existingPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {PermissionId}", PermissionDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            try
            {
                var existingPermission = await _PermissionData.GetByIdAsyncSQL(id);
                if (existingPermission == null)
                {
                    throw new EntityNotFoundException("Permission", id);
                }
                return await _PermissionData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Permission.", ex);
            }
        }

        private void ValidatePermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
            {
                throw new ValidationException("El objeto Permissionas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(PermissionDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private PermissionDTO MapToDTO(Permission Permission)
        {
            return new PermissionDTO
            {
                Id = Permission.Id,
                Name = Permission.Name,

            };
        }

        private Permission MapToEntity(PermissionDTO PermissionDTO)
        {
            return new Permission
            {
                Id = PermissionDTO.Id,
                Name = PermissionDTO.Name,

            };
        }
        private IEnumerable<PermissionDTO> MapToDtoList(IEnumerable<Permission> Permissions)
        {
            var PermissionDto = new List<PermissionDTO>();
            foreach (var Permission in Permissions)
            {
                PermissionDto.Add(MapToDTO(Permission));
            }
            return PermissionDto;
        }


    }
}
