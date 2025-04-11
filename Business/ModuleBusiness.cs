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
    public class ModuleBusiness
    {
        private readonly ModuleData _moduleData;
        private readonly ILogger<ModuleBusiness> _logger;
    
        public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
        {
            _moduleData = moduleData ?? throw new ArgumentNullException(nameof(moduleData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ModuleDTO>> GetAllModuleAsync()
        {
            try
            {
                var module = await _moduleData.GetAllAsyncSQL();

                return MapToDtoList(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los modulos.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de modulos.", ex);
            }
        }

        public async Task<ModuleDTO> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del module debe ser mayor que cero.");
            }

            try
            {
                var module = await _moduleData.GetByIdAsyncSQL(id);
                if (module == null)
                {
                    throw new EntityNotFoundException("module", id);
                }
                return MapToDTO(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ID: {moduleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el modulo con ID {id}.", ex);
            }
        }

        public async Task<ModuleDTO> CreateModuleAsync(ModuleDTO moduleDTO)
        {
            try
            {
                ValidatePerson(moduleDTO);
                var module = MapToEntity(moduleDTO);
                var createdModule = await _moduleData.CreateAsyncSQL(module);
                return MapToDTO(createdModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo modules: {Modulename}", moduleDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el modules.", ex);
            }
        }

        public async Task<bool> UpdateModuleAsync(ModuleDTO moduleDTO)
        {
            try
            {
                ValidatePerson(moduleDTO);
                var existingPerson = await _moduleData.GetByIdAsyncSQL(moduleDTO.Id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", moduleDTO.Id);
                }

                existingPerson.Name = moduleDTO.Name;
              

                return await _moduleData.UpdateAsyncSQL(existingPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {PersonId}", moduleDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            try
            {
                var existingPerson = await _moduleData.GetByIdAsyncSQL(id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Modulo", id);
                }
                return await _moduleData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Module.", ex);
            }
        }

        private void ValidatePerson(ModuleDTO moduleDTO)
        {
            if (moduleDTO == null)
            {
                throw new ValidationException("El objeto personas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(moduleDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private ModuleDTO MapToDTO(Modules module)
        {
            return new ModuleDTO
            {
                Id = module.Id,
                Name = module.Name
      
            };
        }

        private Modules MapToEntity(ModuleDTO moduleDTO)
        {
            return new Modules
            {
                Id = moduleDTO.Id,
                Name = moduleDTO.Name

            };
        }
        private IEnumerable<ModuleDTO> MapToDtoList(IEnumerable<Modules> modules)
        {
            var moduleDto = new List<ModuleDTO>();
            foreach (var module in modules)
            {
                moduleDto.Add(MapToDTO(module));
            }
            return moduleDto;
        }


    }
}
