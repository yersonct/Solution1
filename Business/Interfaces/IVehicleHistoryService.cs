using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVehicleHistoryService
    {
        Task<IEnumerable<VehicleHistoryDTO>> GetAllVehicleHistoriesAsync();
        Task<VehicleHistoryDTO?> GetVehicleHistoryByIdAsync(int id);
        Task<int> CreateVehicleHistoryAsync(VehicleHistoryCreateDTO vehicleHistory);
        Task<bool> UpdateVehicleHistoryAsync(int id, VehicleHistoryCreateDTO vehicleHistory);
        Task<bool> DeleteVehicleHistoryAsync(int id);
    }
}
