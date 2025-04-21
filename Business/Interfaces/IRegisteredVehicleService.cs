using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRegisteredVehicleService
    {
        Task<IEnumerable<RegisteredVehicle>> GetAllRegisteredVehiclesAsync();
        Task<RegisteredVehicle?> GetRegisteredVehicleByIdAsync(int id);
        Task<RegisteredVehicle> CreateRegisteredVehicleAsync(RegisteredVehicle registeredVehicle);
        Task<bool> UpdateRegisteredVehicleAsync(RegisteredVehicle registeredVehicle);
        Task<bool> DeleteRegisteredVehicleAsync(int id);
        Task<bool> CanConnectAsync();
    }
}
