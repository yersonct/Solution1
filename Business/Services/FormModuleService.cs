using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
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
        private readonly ILogger<FormModuleService> _logger;

        public FormModuleService(IFormModuleRepository formModuleRepository, ILogger<FormModuleService> logger)
        {
            _formModuleRepository = formModuleRepository ?? throw new ArgumentNullException(nameof(formModuleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormModuleDTO>> GetAllFormModulesAsync()
        {
            return await _formModuleRepository.GetAllAsync();
        }

        public async Task<FormModuleDTO?> GetFormModuleByIdAsync(int id)
        {
            return await _formModuleRepository.GetByIdAsync(id);
        }

        public async Task<FormModuleDTO> CreateFormModuleAsync(FormModuleCreateDTO formModule)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la relación Form-Module
            return await _formModuleRepository.AddAsync(formModule);
        }

        public async Task<bool> UpdateFormModuleAsync(int id, FormModuleCreateDTO formModule)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la relación Form-Module
            return await _formModuleRepository.UpdateAsync(id, formModule);
        }

        public async Task<bool> DeleteFormModuleAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la relación Form-Module
            return await _formModuleRepository.DeleteAsync(id);
        }
    }
}