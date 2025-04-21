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
                _logger.LogError(ex, "Error al obtener todos los permisos de rol de formulario.");
                throw; // Re-lanza la excepción para que el controlador la maneje
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
                _logger.LogError(ex, "Error al obtener el permiso de rol de formulario por ID: {Id}", id);
                throw; // Re-lanza la excepción
            }
        }

        public async Task<FormRolPermission> CreateFormRolPermissionAsync(FormRolPermission entity)
        {
            try
            {
                // Convierte la entidad a DTO antes de pasarla al repositorio
                var dto = new FormRolPermissionCreateDTO
                {
                    id = entity.id,
                    id_forms = entity.id_forms,
                    id_rol = entity.id_rol,
                    id_permission = entity.id_permission,
                };
                // Llama al repositorio con el DTO
                var createdDto = await _formRolPermissionRepository.AddAsync(dto);

                // Construye y devuelve la entidad FormRolPermission a partir del DTO creado.
                var createdEntity = new FormRolPermission
                {
                    id = createdDto.id,
                    id_forms = createdDto.id_forms,
                    id_rol = createdDto.id_rol,
                    id_permission = createdDto.id_permission,
                    // Aquí se necesitaría lógica adicional para obtener los objetos Forms, Rol y Permission
                };
                return createdEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso de rol de formulario.");
                throw; // Re-lanza la excepción
            }
        }

        public async Task<bool> UpdateFormRolPermissionAsync(FormRolPermission entity)
        {
            try
            {
                // Convierte la entidad a DTO para actualizarla en el repositorio
                var dto = new FormRolPermissionDTO
                {
                    id = entity.id,
                    id_forms = entity.id_forms,
                    id_rol = entity.id_rol,
                    id_permission = entity.id_permission,
                };
                // Llama al repositorio con el DTO
                var result = await _formRolPermissionRepository.UpdateAsync(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso de rol de formulario.");
                throw; // Re-lanza la excepción
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
                _logger.LogError(ex, "Error al eliminar el permiso de rol de formulario con ID: {Id}", id);
                throw; // Re-lanza la excepción
            }
        }
    }
}
