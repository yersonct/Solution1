using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRegisteredVehicleRepository
    {
        Task<bool> CanConnectAsync();
        Task<RegisteredVehicle> AddAsync(RegisteredVehicle entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RegisteredVehicle>> GetAllAsync();
        Task<RegisteredVehicle?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(RegisteredVehicle entity);
    }
}
