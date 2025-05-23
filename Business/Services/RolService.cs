// Business/Services/RolService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio (ej. LogicValidations)
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;

namespace Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogger<RolService> _logger;
        private readonly IMapper _mapper;

        public RolService(IRolRepository rolRepository, ILogger<RolService> logger, IMapper mapper)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RolDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("Obteniendo todos los roles para DTOs.");
            var roles = await _rolRepository.GetAllAsync(); // Obtiene entidades Rol
            return _mapper.Map<IEnumerable<RolDTO>>(roles); // Mapea la colección de entidades a DTOs
        }

        public async Task<RolDTO?> GetRolByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo rol con ID {Id} para DTO.", id);
            var rol = await _rolRepository.GetByIdAsync(id); // Obtiene la entidad Rol
            if (rol == null)
            {
                _logger.LogWarning("Rol con ID {Id} no encontrado en el repositorio.", id);
                return null;
            }
            return _mapper.Map<RolDTO>(rol); // Mapea la entidad a DTO
        }

        public async Task<RolDTO> CreateRolAsync(RolCreateUpdateDTO rolCreateDto)
        {
            _logger.LogInformation("Creando nuevo rol desde DTO.");

            // Validaciones de negocio (ej. nombre único si es un requisito)
            // Example: var existingRoles = await _rolRepository.GetAllAsync();
            // LogicValidations.EnsureRolNameIsUnique(existingRoles, rolCreateDto.Name);

            var rol = _mapper.Map<Rol>(rolCreateDto); // Mapea DTO de creación a entidad
            rol.Active = rolCreateDto.Active; // Usar el valor de Active del DTO

            var createdRol = await _rolRepository.AddAsync(rol); // Guarda la entidad
            _logger.LogInformation("Rol con ID {Id} creado en la base de datos.", createdRol.Id);

            return _mapper.Map<RolDTO>(createdRol); // Mapea la entidad creada a DTO para devolver
        }

        public async Task<bool> UpdateRolAsync(int id, RolCreateUpdateDTO rolUpdateDto)
        {
            _logger.LogInformation("Actualizando rol con ID {Id} desde DTO.", id);
            var existingRol = await _rolRepository.GetByIdAsync(id);
            if (existingRol == null)
            {
                _logger.LogWarning("Intento de actualizar rol con ID {Id} falló: no encontrado.", id);
                return false;
            }

            // Validaciones de negocio antes de actualizar (ej. si el nombre del rol es único y se está cambiando)
            // if (existingRol.Name != rolUpdateDto.Name)
            // {
            //     var allRoles = await _rolRepository.GetAllAsync();
            //     LogicValidations.EnsureRolNameIsUnique(allRoles.Where(r => r.Id != id), rolUpdateDto.Name);
            // }

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(rolUpdateDto, existingRol); // AutoMapper actualizará las propiedades coincidentes

            var result = await _rolRepository.UpdateAsync(existingRol);
            if (result)
            {
                _logger.LogInformation("Rol con ID {Id} actualizado exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar rol con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de rol con ID {Id}.", id);
            var rolToDelete = await _rolRepository.GetByIdAsync(id);
            if (rolToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de rol con ID {Id} falló: no encontrado.", id);
                return false;
            }

            rolToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _rolRepository.UpdateAsync(rolToDelete);
            if (result)
            {
                _logger.LogInformation("Rol con ID {Id} eliminado lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de rol con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        // CONSIDERACIONES:
        // - Implementación de LogicValidations: Asegúrate de que LogicValidations lance excepciones claras
        //   que puedan ser capturadas por tu middleware de errores.
    }
}