using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class FormModuleBusiness
    {
        private readonly FormModuleData _formModuleData;
        private readonly ILogger<FormModuleBusiness> _logger;

        public FormModuleBusiness(FormModuleData formModuleData, ILogger<FormModuleBusiness> logger)
        {
            _formModuleData = formModuleData ?? throw new ArgumentNullException(nameof(formModuleData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormModuleDTO>> GetAllFormModulesAsync()
        {
            try
            {
                return await _formModuleData.GetAllAsyncSQL();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormModules.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de FormModules.", ex);
            }
        }

        public async Task<FormModuleDTO> GetFormModuleByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            try
            {
                var formModule = await _formModuleData.GetByIdAsyncSQL(id);
                if (formModule == null)
                    throw new EntityNotFoundException("FormModule", id);

                return formModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el FormModule con ID: {FormModuleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el FormModule con ID {id}.", ex);
            }
        }

        public async Task<FormModuleDTO> CreateFormModuleAsync(FormModuleCreateDTO createDTO)
        {
            try
            {
                ValidateFormModuleCreateDTO(createDTO);
                return await _formModuleData.CreateAsyncSQL(createDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el FormModule. FormId: {FormId}, ModuleId: {ModuleId}", createDTO.FormId, createDTO.ModuleId);
                throw new ExternalServiceException("Base de datos", "Error al crear el FormModule.", ex);
            }
        }

        public async Task<bool> UpdateFormModuleAsync(int id, FormModuleCreateDTO updateDTO)
        {
            try
            {
                ValidateFormModuleCreateDTO(updateDTO);
                var existing = await _formModuleData.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("FormModule", id);

                return await _formModuleData.UpdateAsyncSQL(id, updateDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el FormModule con ID: {FormModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el FormModule.", ex);
            }
        }

        public async Task<bool> DeleteFormModuleAsync(int id)
        {
            try
            {
                var existing = await _formModuleData.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("FormModule", id);

                return await _formModuleData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el FormModule con ID: {FormModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el FormModule.", ex);
            }
        }

        private void ValidateFormModuleCreateDTO(FormModuleCreateDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto FormModuleCreateDTO no puede ser nulo.");

            if (dto.FormId <= 0)
                throw new ValidationException("FormId", "Debe especificar un formulario válido.");

            if (dto.ModuleId <= 0)
                throw new ValidationException("ModuleId", "Debe especificar un módulo válido.");
        }
    }
}
