using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITypeVehicleService
    {
        Task<IEnumerable<TypeVehicle>> GetAllTypeVehiclesAsync();
        Task<TypeVehicle?> GetTypeVehicleByIdAsync(int id);
        Task<TypeVehicle> CreateTypeVehicleAsync(TypeVehicle typeVehicle);
        Task<bool> UpdateTypeVehicleAsync(TypeVehicle typeVehicle);
        Task<bool> DeleteTypeVehicleAsync(int id);
    }
}
