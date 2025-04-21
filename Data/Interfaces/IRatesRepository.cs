using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRatesRepository
    {
        Task<Rates> AddAsync(Rates entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Rates>> GetAllAsync();
        Task<Rates?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Rates entity);
    }
}
