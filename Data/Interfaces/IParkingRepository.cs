using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IParkingRepository
    {
        Task<Parking> AddAsync(Parking entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Parking>> GetAllAsync();
        Task<Parking?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Parking entity);
    }
}
