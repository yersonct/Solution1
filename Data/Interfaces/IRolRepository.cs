using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolRepository
    {
        Task<Rol> AddAsync(Rol entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Rol entity);
    }
}
