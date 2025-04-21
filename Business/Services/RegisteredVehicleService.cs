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
    public class RegisteredVehicleService : IRegisteredVehicleService
    {
        private readonly IRegisteredVehicleRepository _registeredVehicleRepository;
        private readonly ILogger<RegisteredVehicleService> _logger;

        public RegisteredVehicleService(IRegisteredVehicleRepository registeredVehicleRepository, ILogger<RegisteredVehicleService> logger)
        {
            _registeredVehicleRepository = registeredVehicleRepository ?? throw new ArgumentNullException(nameof(registeredVehicleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CanConnectAsync()
        {
            return await _registeredVehicleRepository.CanConnectAsync();
        }

        public async Task<IEnumerable<RegisteredVehicle>> GetAllRegisteredVehiclesAsync()
        {
            return await _registeredVehicleRepository.GetAllAsync();
        }

        public async Task<RegisteredVehicle?> GetRegisteredVehicleByIdAsync(int id)
        {
            return await _registeredVehicleRepository.GetByIdAsync(id);
        }

        public async Task<RegisteredVehicle> CreateRegisteredVehicleAsync(RegisteredVehicle registeredVehicle)
        {
            //  Add business logic here before creating a registered vehicle
            return await _registeredVehicleRepository.AddAsync(registeredVehicle);
        }

        public async Task<bool> UpdateRegisteredVehicleAsync(RegisteredVehicle registeredVehicle)
        {
            //  Add business logic here before updating a registered vehicle
            return await _registeredVehicleRepository.UpdateAsync(registeredVehicle);
        }

        public async Task<bool> DeleteRegisteredVehicleAsync(int id)
        {
            //  Add business logic here before deleting a registered vehicle
            return await _registeredVehicleRepository.DeleteAsync(id);
        }
    }
}
