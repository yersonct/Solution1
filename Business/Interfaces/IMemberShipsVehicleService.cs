using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMembershipsVehicleService
    {
        Task<IEnumerable<MembershipsVehicle>> GetAllMembershipsVehiclesAsync();
        Task<MembershipsVehicle?> GetMembershipsVehicleByIdAsync(int id);
        Task<MembershipsVehicle> CreateMembershipsVehicleAsync(MembershipsVehicle membershipsVehicle);
        Task<bool> UpdateMembershipsVehicleAsync(MembershipsVehicle membershipsVehicle);
        Task<bool> DeleteMembershipsVehicleAsync(int id);
    }
}
