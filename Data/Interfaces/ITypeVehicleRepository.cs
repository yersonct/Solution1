using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITypeVehicleRepository
    {
        Task<TypeVehicle> AddAsync(TypeVehicle entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TypeVehicle>> GetAllAsync();
        Task<TypeVehicle?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(TypeVehicle entity);
    }
}
