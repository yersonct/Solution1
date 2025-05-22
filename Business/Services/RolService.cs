using Business.Interfaces;
using Business.Validations;
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
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogger<RolService> _logger;

        public RolService(IRolRepository rolRepository, ILogger<RolService> logger)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Rol>> GetAllRolesAsync()
        {
            return await _rolRepository.GetAllAsync();
        }

        public async Task<Rol?> GetRolByIdAsync(int id)
        {
            return await _rolRepository.GetByIdAsync(id);
        }

        public async Task<Rol> CreateRolAsync(Rol rol)
        {
            LogicValidations.ValidateRolName(rol.name, _logger);
            LogicValidations.ValidateRolDescription(rol.description, _logger);
            // Aquí podrías agregar lógica de negocio antes de crear el rol
            // No es necesario establecer Active a true aquí si ya lo haces en el repositorio
            return await _rolRepository.AddAsync(rol);
        }

        public async Task<bool> UpdateRolAsync(Rol rol)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el rol
            var existingRol = await _rolRepository.GetByIdAsync(rol.id);
            LogicValidations.ValidateExistingRol(existingRol, rol.id, _logger);

            // Validaciones usando la clase LogicValidations
            LogicValidations.ValidateRolName(rol.name, _logger);
            LogicValidations.ValidateRolDescription(rol.description, _logger);
            return await _rolRepository.UpdateAsync(rol);
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el rol
            var existingRol = await _rolRepository.GetByIdAsync(id);
            LogicValidations.ValidateExistingRol(existingRol, id, _logger);
            return await _rolRepository.DeleteAsync(id);
        }
    }
}