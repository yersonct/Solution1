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
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILogger<ModuleService> _logger;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
        {
            _moduleRepository = moduleRepository ?? throw new ArgumentNullException(nameof(moduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Modules>> GetAllModulesAsync()
        {
            return await _moduleRepository.GetAllAsync();
        }

        public async Task<Modules?> GetModuleByIdAsync(int id)
        {
            return await _moduleRepository.GetByIdAsync(id);
        }

        public async Task<Modules> CreateModuleAsync(Modules module)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el módulo
            return await _moduleRepository.AddAsync(module);
        }

        public async Task<bool> UpdateModuleAsync(Modules module)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el módulo
            return await _moduleRepository.UpdateAsync(module);
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el módulo
            return await _moduleRepository.DeleteAsync(id);
        }
    }
}
