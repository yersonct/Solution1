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
            // Aquí podrías agregar lógica de negocio antes de crear el rol
            // No es necesario establecer Active a true aquí si ya lo haces en el repositorio
            return await _rolRepository.AddAsync(rol);
        }

        public async Task<bool> UpdateRolAsync(Rol rol)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el rol
            return await _rolRepository.UpdateAsync(rol);
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el rol
            return await _rolRepository.DeleteAsync(id);
        }
    }
}