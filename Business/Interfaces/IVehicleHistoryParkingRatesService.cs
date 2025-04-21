using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVehicleHistoryParkingRatesService
    {
        Task<IEnumerable<VehicleHistoryParkingRates>> GetAllVehicleHistoryParkingRatesAsync();
        Task<VehicleHistoryParkingRates?> GetVehicleHistoryParkingRatesByIdAsync(int id);
        Task<VehicleHistoryParkingRates> CreateVehicleHistoryParkingRatesAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates);
        Task<bool> UpdateVehicleHistoryParkingRatesAsync(VehicleHistoryParkingRates vehicleHistoryParkingRates);
        Task<bool> DeleteVehicleHistoryParkingRatesAsync(int id);
    }
}
