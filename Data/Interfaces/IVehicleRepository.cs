using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> AddAsync(Vehicle entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Vehicle entity);
    }
}
