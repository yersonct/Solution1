// Business/VehicleHistoryParkingRatesBusiness.cs
using Data;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class VehicleHistoryParkingRatesBusiness
    {
        private readonly VehicleHistoryParkingRatesData _data;
        private readonly ILogger<VehicleHistoryParkingRatesBusiness> _logger;

        public VehicleHistoryParkingRatesBusiness(VehicleHistoryParkingRatesData data, ILogger<VehicleHistoryParkingRatesBusiness> logger)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetAllAsync()
        {
            try
            {
                return await _data.GetAllAsyncLINQ(); // O GetAllAsyncSQL(), elige la implementación que prefieras
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistoryParkingRates.");
                throw;
            }
        }

        public async Task<VehicleHistoryParkingRates?> GetByIdAsync(int id)
        {
            try
            {
                return await _data.GetByIdAsyncLINQ(id); // O GetByIdAsyncSQL(id)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistoryParkingRates con ID {Id}.", id);
                throw;
            }
        }

        public async Task<VehicleHistoryParkingRates> CreateAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                return await _data.CreateAsyncLINQ(vehicleHistoryParkingRates); // O CreateAsyncSQL(vehicleHistoryParkingRates)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistoryParkingRates.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                return await _data.UpdateAsyncLINQ(vehicleHistoryParkingRates); // O UpdateAsyncSQL(vehicleHistoryParkingRates)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistoryParkingRates con ID {Id}.", vehicleHistoryParkingRates.id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _data.DeleteAsyncLINQ(id); // O DeleteAsyncSQL(id)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistoryParkingRates con ID {Id}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByVehicleHistoryIdAsync(int vehicleHistoryId)
        {
            try
            {
                return await _data.GetByVehicleHistoryIdAsync(vehicleHistoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros por VehicleHistoryId {VehicleHistoryId}.", vehicleHistoryId);
                throw;
            }
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByParkingIdAsync(int parkingId)
        {
            try
            {
                return await _data.GetByParkingIdAsync(parkingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros por ParkingId {ParkingId}.", parkingId);
                throw;
            }
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByRatesIdAsync(int ratesId)
        {
            try
            {
                return await _data.GetByRatesIdAsync(ratesId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros por RatesId {RatesId}.", ratesId);
                throw;
            }
        }
    }
}