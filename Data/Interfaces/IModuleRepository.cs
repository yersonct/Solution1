using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IModuleRepository
    {
        Task<Modules> AddAsync(Modules entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Modules>> GetAllAsync();
        Task<Modules?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Modules entity);
    }
}
