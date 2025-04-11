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
    public class FormBusiness
    {
        private readonly FormData _formsData;
        private readonly ILogger<FormBusiness> _logger;

        public FormBusiness(FormData formsData, ILogger<FormBusiness> logger)
        {
            _formsData = formsData ?? throw new ArgumentNullException(nameof(formsData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormDTO>> GetAllFormsAsync()
        {
            try
            {
                var forms = await _formsData.GetAllAsyncSQL();

                return MapToDtoList(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los modulos.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de modulos.", ex);
            }
        }

        public async Task<FormDTO> GetFormsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del module debe ser mayor que cero.");
            }

            try
            {
                var forms   = await _formsData.GetByIdAsyncSQL(id);
                if (forms == null)
                {
                    throw new EntityNotFoundException("module", id);
                }
                return MapToDTO(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ID: {moduleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el modulo con ID {id}.", ex);
            }
        }

        public async Task<FormDTO> CreateFormsAsync(FormDTO formDTO)
        {
            try
            {
                ValidateForms(formDTO);
                var forms = MapToEntity(formDTO);
                var createdForms = await _formsData.CreateAsyncSQL(forms);
                return MapToDTO(createdForms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo modules: {Modulename}", formDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el modules.", ex);
            }
        }

        public async Task<bool> UpdateFormAsync(FormDTO formDTO)
        {
            try
            {
                ValidateForms(formDTO);
                var existingform = await _formsData.GetByIdAsyncSQL(formDTO.Id);
                if (existingform == null)
                {
                    throw new EntityNotFoundException("Person", formDTO.Id);
                }

                existingform.Name = formDTO.Name;


                return await _formsData.UpdateAsyncSQL(existingform);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {PersonId}", formDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            try
            {
                var existingform = await _formsData.GetByIdAsyncSQL(id);
                if (existingform == null)
                {
                    throw new EntityNotFoundException("form", id);
                }
                return await _formsData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Module.", ex);
            }
        }

        private void ValidateForms(FormDTO formDTO)
        {
            if (formDTO == null)
            {
                throw new ValidationException("El objeto personas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(formDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private FormDTO MapToDTO(Forms forms)
        {
            return new FormDTO
            {
                Id = forms.Id,
                Name = forms.Name,
                Url = forms.Url

            };
        }

        private Forms MapToEntity(FormDTO formDTO)
        {
            return new Forms
            {
                Id = formDTO.Id,
                Name = formDTO.Name,
                Url = formDTO.Url,

            };
        }
        private IEnumerable<FormDTO> MapToDtoList(IEnumerable<Forms> forms)
        {
            var FormDto = new List<FormDTO>();
            foreach (var form in forms)
            {
                FormDto.Add(MapToDTO(form));
            }
            return FormDto;
        }


    }
}
