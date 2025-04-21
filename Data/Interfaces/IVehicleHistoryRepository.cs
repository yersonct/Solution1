using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVehicleHistoryRepository
    {
        Task<IEnumerable<VehicleHistoryDTO>> GetAllAsync();
        Task<VehicleHistoryDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(VehicleHistoryCreateDTO entity);
        Task<bool> UpdateAsync(int id, VehicleHistoryCreateDTO entity);
        Task<bool> DeleteAsync(int id);
    }
}
