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
    public class VehicleHistoryService : IVehicleHistoryService
    {
        private readonly IVehicleHistoryRepository _vehicleHistoryRepository;
        private readonly ILogger<VehicleHistoryService> _logger;

        public VehicleHistoryService(IVehicleHistoryRepository vehicleHistoryRepository, ILogger<VehicleHistoryService> logger)
        {
            _vehicleHistoryRepository = vehicleHistoryRepository ?? throw new ArgumentNullException(nameof(vehicleHistoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<VehicleHistoryDTO>> GetAllVehicleHistoriesAsync()
        {
            return await _vehicleHistoryRepository.GetAllAsync();
        }

        public async Task<VehicleHistoryDTO?> GetVehicleHistoryByIdAsync(int id)
        {
            return await _vehicleHistoryRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateVehicleHistoryAsync(VehicleHistoryCreateDTO vehicleHistory)
        {
            //  Add business logic before creating
            return await _vehicleHistoryRepository.AddAsync(vehicleHistory);
        }

        public async Task<bool> UpdateVehicleHistoryAsync(int id, VehicleHistoryCreateDTO vehicleHistory)
        {
            //  Add business logic before updating
            return await _vehicleHistoryRepository.UpdateAsync(id, vehicleHistory);
        }

        public async Task<bool> DeleteVehicleHistoryAsync(int id)
        {
            //  Add business logic before deleting
            return await _vehicleHistoryRepository.DeleteAsync(id);
        }
    }
}
