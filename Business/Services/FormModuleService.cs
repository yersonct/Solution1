using AutoMapper; // Necesario para AutoMapper
using Business.Interfaces;
using Business.Validations; // Mantén LogicValidations
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model; // Necesario para las entidades del modelo
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FormModuleService : IFormModuleService
    {
        private readonly IFormModuleRepository _formModuleRepository;
        private readonly IMapper _mapper; // Inyectamos IMapper
        private readonly ILogger<FormModuleService> _logger;

        public FormModuleService(IFormModuleRepository formModuleRepository,
                                 IMapper mapper, // Añadir IMapper al constructor
                                 ILogger<FormModuleService> logger)
        {
            _formModuleRepository = formModuleRepository ?? throw new ArgumentNullException(nameof(formModuleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); // Inicializar IMapper
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormModuleDTO>> GetAllFormModulesAsync()
        {
            // El repositorio devuelve entidades (FormModule)
            var formModules = await _formModuleRepository.GetAllAsync();
            // Mapeamos la colección de entidades a una colección de DTOs
            return _mapper.Map<IEnumerable<FormModuleDTO>>(formModules);
        }

        public async Task<FormModuleDTO?> GetFormModuleByIdAsync(int id)
        {
            // El repositorio devuelve una entidad (FormModule)
            var formModule = await _formModuleRepository.GetByIdAsync(id);
            // Mapeamos la entidad a un DTO
            return _mapper.Map<FormModuleDTO>(formModule);
        }

        public async Task<FormModuleDTO> CreateFormModuleAsync(FormModuleCreateDTO formModuleDto)
        {
            // Validaciones con LogicValidations (que recibe DTO)
            LogicValidations.ValidateFormModule(formModuleDto, _logger);

            // Mapeamos el DTO de creación a la entidad del modelo
            var formModuleEntity = _mapper.Map<FormModule>(formModuleDto);

            // El repositorio agrega la entidad
            var createdEntity = await _formModuleRepository.AddAsync(formModuleEntity);

            // Mapeamos la entidad creada de vuelta a un DTO para el retorno
            return _mapper.Map<FormModuleDTO>(createdEntity);
        }

        public async Task<bool> UpdateFormModuleAsync(int id, FormModuleCreateDTO formModuleDto)
        {
            // Obtener la entidad existente del repositorio (asumiendo que devuelve FormModule)
            var existingFormModule = await _formModuleRepository.GetByIdAsync(id);

            // Validar la existencia de la entidad (LogicValidations espera una entidad o null, no DTO)
            // Aquí hay un cambio: LogicValidations.ValidateExistingFormModule() debe recibir una entidad 'FormModule'
            // O debes adaptar tu LogicValidations para que reciba FormModuleDTO y haga la validación de existencia.
            // Por simplicidad, asumo que 'GetByIdAsync(id)' de _formModuleRepository ahora devuelve 'FormModule'.
            LogicValidations.ValidateExistingFormModule(existingFormModule, id, _logger); // Necesitarías ajustar esta validación o el tipo que espera.
            if (existingFormModule == null) return false; // Si no existe, no se puede actualizar.

            // Validaciones con LogicValidations (que recibe DTO)
            LogicValidations.ValidateFormModule(formModuleDto, _logger);

            // Mapear los campos del DTO de actualización a la entidad existente
            // AutoMapper es inteligente y actualizará la entidad existente con los valores del DTO.
            _mapper.Map(formModuleDto, existingFormModule);

            // El repositorio actualiza la entidad
            return await _formModuleRepository.UpdateAsync(existingFormModule); // Asumo que el UpdateAsync recibe una entidad
        }

        public async Task<bool> DeleteFormModuleAsync(int id)
        {
            // Obtener la entidad existente del repositorio
            var existingFormModule = await _formModuleRepository.GetByIdAsync(id);
            // Validar la existencia (ajustar LogicValidations si es necesario)
            LogicValidations.ValidateExistingFormModule(existingFormModule, id, _logger);
            if (existingFormModule == null) return false;

            // Aquí puedes decidir si quieres hacer borrado lógico (actualizar el campo Active) o físico.
            // Para borrado lógico:
            existingFormModule.Active = false;
            return await _formModuleRepository.UpdateAsync(existingFormModule);
            // Para borrado físico:
            // return await _formModuleRepository.DeleteAsync(id);
        }
    }
}