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
    public class RolUserService : IRolUserService
    {
        private readonly IRolUserRepository _rolUserRepository;
        private readonly ILogger<RolUserService> _logger;

        public RolUserService(IRolUserRepository rolUserRepository, ILogger<RolUserService> logger)
        {
            _rolUserRepository = rolUserRepository ?? throw new ArgumentNullException(nameof(rolUserRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RolUser>> GetAllRolUsersAsync()
        {
            return await _rolUserRepository.GetAllAsync();
        }

        public async Task<RolUser?> GetRolUserByIdAsync(int id)
        {
            return await _rolUserRepository.GetByIdAsync(id);
        }

        public async Task<RolUser> CreateRolUserAsync(RolUser rolUser)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la relación Rol-Usuario
            return await _rolUserRepository.AddAsync(rolUser);
        }

        public async Task<bool> UpdateRolUserAsync(RolUser rolUser)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la relación Rol-Usuario
            return await _rolUserRepository.UpdateAsync(rolUser);
        }

        public async Task<bool> DeleteRolUserAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la relación Rol-Usuario
            return await _rolUserRepository.DeleteAsync(id);
        }
    }
}
