using Business.Interfaces;
using Business.Validations;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly ILogger<FormService> _logger;

        public FormService(IFormRepository formRepository, ILogger<FormService> logger)
        {
            _formRepository = formRepository ?? throw new ArgumentNullException(nameof(formRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Forms>> GetAllFormsAsync()
        {
              try
            {
                return await _formRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios.");
                throw new ArgumentException("Error al acceder a los datos de los formularios.", ex);
            }
        }

        public async Task<Forms?> GetFormByIdAsync(int id)
        {
            try
            {
                var form = await _formRepository.GetByIdAsync(id);
                if (form == null)
                {
                    _logger.LogWarning($"No se encontró el formulario con ID: {id}.");
                    throw new NotFoundException($"Formulario con ID {id} no encontrado.");
                }
                return form;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el formulario con ID: {id}.");
                throw new ArgumentException("Error al acceder a los datos del formulario.", ex);
            }
        }

        public async Task<Forms> CreateFormAsync(Forms form)
        {
            // You can add business logic here before creating the form
            try
            {
                LogicValidations.FormValidations.ValidateForm(form); // Validar el formulario

                return await _formRepository.AddAsync(form);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación al crear el formulario.");
                throw; // Re-lanzar la excepción de validación
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el formulario.");
                throw new ArgumentException("Error al guardar los datos del formulario.", ex);
            }
        }

        public async Task<bool> UpdateFormAsync(Forms form)
        {
            // You can add business logic here before updating the form
            try
            {
                LogicValidations.FormValidations.ValidateForm(form); // Validar el formulario

                return await _formRepository.UpdateAsync(form);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación al actualizar el formulario.");
                throw; // Re-lanzar la excepción de validación
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el formulario.");
                throw new ArgumentException("Error al actualizar los datos del formulario.", ex);
            }
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            // You can add business logic here before deleting the form
            try
            {
                return await _formRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el formulario con ID: {id}.");
                throw new ArgumentException("Error al eliminar los datos del formulario.", ex);
            }
        }
    }
}