// Business/Services/FormService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio (ej. FormValidations)
using Data.Interfaces;
using Entity.Model; // Asegúrate de que sea 'Form' singular
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;
using SendGrid.Helpers.Errors.Model; // Para NotFoundException si la usas

namespace Business.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly ILogger<FormService> _logger;
        private readonly IMapper _mapper;

        public FormService(IFormRepository formRepository, ILogger<FormService> logger, IMapper mapper)
        {
            _formRepository = formRepository ?? throw new ArgumentNullException(nameof(formRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); // Asegura que mapper no sea nulo
        }

        public async Task<IEnumerable<FormDTO>> GetAllFormsAsync()
        {
            _logger.LogInformation("Obteniendo todos los formularios para DTOs.");
            var forms = await _formRepository.GetAllAsync(); // Obtiene entidades Form
            return _mapper.Map<IEnumerable<FormDTO>>(forms); // Mapea la colección de entidades a DTOs
        }

        public async Task<FormDTO?> GetFormByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo formulario con ID {Id} para DTO.", id);
            var form = await _formRepository.GetByIdAsync(id); // Obtiene la entidad Form
            if (form == null)
            {
                _logger.LogWarning("Formulario con ID {Id} no encontrado en el repositorio.", id);
                // Lanza una excepción que será capturada por el middleware de errores
                throw new NotFoundException($"Formulario con ID {id} no encontrado.");
            }
            return _mapper.Map<FormDTO>(form); // Mapea la entidad a DTO
        }

        public async Task<FormDTO> CreateFormAsync(FormCreateUpdateDTO formCreateDto)
        {
            _logger.LogInformation("Creando nuevo formulario desde DTO.");

            // Validaciones de negocio (ej. nombre único si es un requisito)
            // Example: var existingForms = await _formRepository.GetAllAsync();
            // LogicValidations.FormValidations.EnsureFormNameIsUnique(existingForms, formCreateDto.Name, _logger); // Asegúrate de implementar esto

            var form = _mapper.Map<Forms>(formCreateDto); // ¡Mapea DTO de creación a entidad 'Form' singular!
            form.Active = formCreateDto.Active; // Usar el valor de Active del DTO

            // Si necesitas validar la entidad Form antes de añadirla, puedes usar LogicValidations.FormValidations.ValidateForm(form);
            // Asegúrate de que este método lanza ArgumentException o similar en caso de error.

            var createdForm = await _formRepository.AddAsync(form); // Guarda la entidad
            _logger.LogInformation("Formulario con ID {Id} creado en la base de datos.", createdForm.Id);

            return _mapper.Map<FormDTO>(createdForm); // Mapea la entidad creada a DTO para devolver
        }

        public async Task<bool> UpdateFormAsync(int id, FormCreateUpdateDTO formUpdateDto)
        {
            _logger.LogInformation("Actualizando formulario con ID {Id} desde DTO.", id);
            var existingForm = await _formRepository.GetByIdAsync(id);
            if (existingForm == null)
            {
                _logger.LogWarning("Intento de actualizar formulario con ID {Id} falló: no encontrado.", id);
                return false; // El controlador devolverá NotFound si el servicio devuelve false
            }

            // Validaciones de negocio antes de actualizar (ej. si el nombre del formulario es único y se está cambiando)
            // if (existingForm.Name != formUpdateDto.Name)
            // {
            //     var allForms = await _formRepository.GetAllAsync();
            //     LogicValidations.FormValidations.EnsureFormNameIsUnique(allForms.Where(f => f.Id != id), formUpdateDto.Name, _logger);
            // }

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(formUpdateDto, existingForm); // AutoMapper actualizará las propiedades coincidentes

            // Si necesitas validar la entidad Form antes de actualizarla, puedes usar LogicValidations.FormValidations.ValidateForm(existingForm);

            var result = await _formRepository.UpdateAsync(existingForm);
            if (result)
            {
                _logger.LogInformation("Formulario con ID {Id} actualizado exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar formulario con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de formulario con ID {Id}.", id);
            var formToDelete = await _formRepository.GetByIdAsync(id);
            if (formToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de formulario con ID {Id} falló: no encontrado.", id);
                return false;
            }

            formToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _formRepository.UpdateAsync(formToDelete); // Usar Update para borrado lógico
            if (result)
            {
                _logger.LogInformation("Formulario con ID {Id} eliminado lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de formulario con ID {Id} en el repositorio.", id);
            }
            return result;
        }
    }
}