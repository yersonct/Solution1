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
            rolUser.active = true; // Establecer active a true al crear
            return await _rolUserRepository.AddAsync(rolUser);
        }

        public async Task<bool> UpdateRolUserAsync(RolUser rolUser)
        {
            return await _rolUserRepository.UpdateAsync(rolUser);
        }

        public async Task<bool> DeleteRolUserAsync(int id)
        {
            var rolUserToDelete = await _rolUserRepository.GetByIdAsync(id);
            if (rolUserToDelete == null)
            {
                return false;
            }
            rolUserToDelete.active = false; // Borrado lógico
            return await _rolUserRepository.UpdateAsync(rolUserToDelete);
        }
    }
}