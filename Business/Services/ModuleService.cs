// Business/Services/ModuleService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio (ej. LogicValidations)
using Data.Interfaces;
using Entity.Model; // Asegúrate de que sea 'Module' singular
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;

namespace Business.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILogger<ModuleService> _logger;
        private readonly IMapper _mapper;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger, IMapper mapper)
        {
            _moduleRepository = moduleRepository ?? throw new ArgumentNullException(nameof(moduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ModuleDTO>> GetAllModulesAsync()
        {
            _logger.LogInformation("Obteniendo todos los módulos para DTOs.");
            var modules = await _moduleRepository.GetAllAsync(); // Obtiene entidades Module
            return _mapper.Map<IEnumerable<ModuleDTO>>(modules); // Mapea la colección de entidades a DTOs
        }

        public async Task<ModuleDTO?> GetModuleByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo módulo con ID {Id} para DTO.", id);
            var module = await _moduleRepository.GetByIdAsync(id); // Obtiene la entidad Module
            if (module == null)
            {
                _logger.LogWarning("Módulo con ID {Id} no encontrado en el repositorio.", id);
                return null;
            }
            return _mapper.Map<ModuleDTO>(module); // Mapea la entidad a DTO
        }

        public async Task<ModuleDTO> CreateModuleAsync(ModuleCreateUpdateDTO moduleCreateDto)
        {
            _logger.LogInformation("Creando nuevo módulo desde DTO.");

            // Validaciones de negocio (ej. nombre único si es un requisito)
            // Example: var existingModules = await _moduleRepository.GetAllAsync();
            // LogicValidations.EnsureModuleNameIsUnique(existingModules, moduleCreateDto.Name, _logger); // Asegúrate de implementar esto

            var module = _mapper.Map<Modules>(moduleCreateDto); // ¡CORREGIDO: Mapea a 'Module' singular!
            module.Active = moduleCreateDto.Active; // Usar el valor de Active del DTO

            var createdModule = await _moduleRepository.AddAsync(module); // Guarda la entidad
            _logger.LogInformation("Módulo con ID {Id} creado en la base de datos.", createdModule.Id);

            return _mapper.Map<ModuleDTO>(createdModule); // Mapea la entidad creada a DTO para devolver
        }

        public async Task<bool> UpdateModuleAsync(int id, ModuleCreateUpdateDTO moduleUpdateDto)
        {
            _logger.LogInformation("Actualizando módulo con ID {Id} desde DTO.", id);
            var existingModule = await _moduleRepository.GetByIdAsync(id);
            if (existingModule == null)
            {
                _logger.LogWarning("Intento de actualizar módulo con ID {Id} falló: no encontrado.", id);
                return false;
            }

            // Validaciones de negocio antes de actualizar (ej. si el nombre del módulo es único y se está cambiando)
            // if (existingModule.Name != moduleUpdateDto.Name)
            // {
            //     var allModules = await _moduleRepository.GetAllAsync();
            //     LogicValidations.EnsureModuleNameIsUnique(allModules.Where(m => m.Id != id), moduleUpdateDto.Name, _logger);
            // }

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(moduleUpdateDto, existingModule); // AutoMapper actualizará las propiedades coincidentes

            var result = await _moduleRepository.UpdateAsync(existingModule);
            if (result)
            {
                _logger.LogInformation("Módulo con ID {Id} actualizado exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar módulo con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de módulo con ID {Id}.", id);
            var moduleToDelete = await _moduleRepository.GetByIdAsync(id);
            if (moduleToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de módulo con ID {Id} falló: no encontrado.", id);
                return false;
            }

            moduleToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _moduleRepository.UpdateAsync(moduleToDelete);
            if (result)
            {
                _logger.LogInformation("Módulo con ID {Id} eliminado lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de módulo con ID {Id} en el repositorio.", id);
            }
            return result;
        }
    }
}