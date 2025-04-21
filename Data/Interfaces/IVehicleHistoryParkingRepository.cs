using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVehicleHistoryParkingRatesRepository
    {
        Task<VehicleHistoryParkingRates> AddAsync(VehicleHistoryParkingRates entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<VehicleHistoryParkingRates>> GetAllAsync();
        Task<VehicleHistoryParkingRates?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(VehicleHistoryParkingRates entity);
    }
}
