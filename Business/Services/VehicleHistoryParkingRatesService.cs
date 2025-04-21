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
    public class VehicleHistoryParkingRatesService : IVehicleHistoryParkingRatesService
    {
        private readonly IVehicleHistoryParkingRatesRepository _vhprRepository;
        private readonly ILogger<VehicleHistoryParkingRatesService> _logger;

        public VehicleHistoryParkingRatesService(IVehicleHistoryParkingRatesRepository vhprRepository, ILogger<VehicleHistoryParkingRatesService> logger)
        {
            _vhprRepository = vhprRepository ?? throw new ArgumentNullException(nameof(vhprRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetAllVehicleHistoryParkingRatesAsync()
        {
            return await _vhprRepository.GetAllAsync();
        }

        public async Task<VehicleHistoryParkingRates?> GetVehicleHistoryParkingRatesByIdAsync(int id)
        {
            return await _vhprRepository.GetByIdAsync(id);
        }

        public async Task<VehicleHistoryParkingRates> CreateVehicleHistoryParkingRatesAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            //  Agregar lógica de negocio aquí
            return await _vhprRepository.AddAsync(vehicleHistoryParkingRates);
        }

        public async Task<bool> UpdateVehicleHistoryParkingRatesAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            //  Agregar lógica de negocio aquí
            return await _vhprRepository.UpdateAsync(vehicleHistoryParkingRates);
        }

        public async Task<bool> DeleteVehicleHistoryParkingRatesAsync(int id)
        {
            //  Agregar lógica de negocio aquí
            return await _vhprRepository.DeleteAsync(id);
        }
    }
}
