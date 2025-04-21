using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMembershipsVehicleRepository
    {
        Task<MembershipsVehicle> AddAsync(MembershipsVehicle entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<MembershipsVehicle>> GetAllAsync();
        Task<MembershipsVehicle?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(MembershipsVehicle entity);
    }
}
