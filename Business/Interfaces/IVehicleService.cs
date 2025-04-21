using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(int id);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);
    }
}
