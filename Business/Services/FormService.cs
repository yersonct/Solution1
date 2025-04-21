using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
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
            return await _formRepository.GetAllAsync();
        }

        public async Task<Forms?> GetFormByIdAsync(int id)
        {
            return await _formRepository.GetByIdAsync(id);
        }

        public async Task<Forms> CreateFormAsync(Forms form)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el formulario
            return await _formRepository.AddAsync(form);
        }

        public async Task<bool> UpdateFormAsync(Forms form)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el formulario
            return await _formRepository.UpdateAsync(form);
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el formulario
            return await _formRepository.DeleteAsync(id);
        }
    }
}
