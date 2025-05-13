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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el usuario
            // Asegúrate de que la propiedad 'Active' (si la agregaste a la entidad User) se inicialice en true
            var existingUsers = await _userRepository.GetAllAsync();
            LogicValidations.EnsureUserNameIsUnique(existingUsers, user.username);

            user.active = true;
            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el usuario
            var existingUser = await _userRepository.GetByIdAsync(user.id);
            LogicValidations.EnsureUserExists(existingUser);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el usuario
            return await _userRepository.DeleteAsync(id);
        }
    }
}