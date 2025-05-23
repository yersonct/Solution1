// Business/Services/PermissionService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq; // Mantener si usas LINQ para algo más allá del mapeo
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;

namespace Business.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<PermissionService> _logger;
        private readonly IMapper _mapper;

        public PermissionService(
            IPermissionRepository permissionRepository,
            ILogger<PermissionService> logger,
            IMapper mapper)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            _logger.LogInformation("Obteniendo todos los permisos para DTOs.");
            var permissions = await _permissionRepository.GetAllAsync(); // Obtiene entidades Permission
            return _mapper.Map<IEnumerable<PermissionDTO>>(permissions); // Mapea la colección de entidades a DTOs
        }

        public async Task<PermissionDTO?> GetPermissionByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo permiso con ID {Id} para DTO.", id);
            var permission = await _permissionRepository.GetByIdAsync(id); // Obtiene la entidad Permission
            if (permission == null)
            {
                _logger.LogWarning("Permiso con ID {Id} no encontrado en el repositorio.", id);
                return null;
            }
            return _mapper.Map<PermissionDTO>(permission); // Mapea la entidad a DTO
        }

        public async Task<PermissionDTO> CreatePermissionAsync(PermissionCreateUpdateDTO permissionCreateDto)
        {
            _logger.LogInformation("Creando nuevo permiso desde DTO.");

            // Puedes agregar validaciones de negocio aquí, por ejemplo, asegurar que el nombre del permiso sea único
            var existingPermissions = await _permissionRepository.GetAllAsync();
            // LogicValidations.ValidatePermissionName(permissionCreateDto.Name, existingPermissions, _logger); // Asegúrate de implementar esta validación en LogicValidations

            var permission = _mapper.Map<Permission>(permissionCreateDto); // Mapea DTO de creación a entidad
            permission.Active = permissionCreateDto.Active; // Usa el valor de Active del DTO

            var createdPermission = await _permissionRepository.AddAsync(permission); // Guarda la entidad
            _logger.LogInformation("Permiso con ID {Id} creado en la base de datos.", createdPermission.Id);

            return _mapper.Map<PermissionDTO>(createdPermission); // Mapea la entidad creada a DTO para devolver
        }

        public async Task<bool> UpdatePermissionAsync(int id, PermissionCreateUpdateDTO permissionUpdateDto)
        {
            _logger.LogInformation("Actualizando permiso con ID {Id} desde DTO.", id);
            var existingPermission = await _permissionRepository.GetByIdAsync(id);
            if (existingPermission == null)
            {
                _logger.LogWarning("Intento de actualizar permiso con ID {Id} falló: no encontrado.", id);
                return false;
            }

            // Validaciones de negocio antes de actualizar (ej. si el nombre del permiso es único y se está cambiando)
            // if (existingPermission.Name != permissionUpdateDto.Name)
            // {
            //     var allPermissions = await _permissionRepository.GetAllAsync();
            //     LogicValidations.ValidatePermissionName(permissionUpdateDto.Name, allPermissions.Where(p => p.Id != id), _logger);
            // }

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(permissionUpdateDto, existingPermission); // AutoMapper actualizará las propiedades coincidentes

            var result = await _permissionRepository.UpdateAsync(existingPermission);
            if (result)
            {
                _logger.LogInformation("Permiso con ID {Id} actualizado exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar permiso con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de permiso con ID {Id}.", id);
            var permissionToDelete = await _permissionRepository.GetByIdAsync(id);
            if (permissionToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de permiso con ID {Id} falló: no encontrado.", id);
                return false;
            }

            permissionToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _permissionRepository.UpdateAsync(permissionToDelete);
            if (result)
            {
                _logger.LogInformation("Permiso con ID {Id} eliminado lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de permiso con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        // CONSIDERACIONES:
        // - LogicValidations: Asegúrate de que los métodos en esta clase lancen excepciones claras
        //   que puedan ser manejadas por tu middleware global de errores.
    }
}