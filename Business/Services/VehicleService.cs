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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(IVehicleRepository vehicleRepository, ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el vehículo
            return await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el vehículo
            return await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el vehículo
            return await _vehicleRepository.DeleteAsync(id);
        }
    }
}
