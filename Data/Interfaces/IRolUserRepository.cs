using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolUserRepository
    {
        Task<RolUser> AddAsync(RolUser entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RolUser>> GetAllAsync();
        Task<RolUser?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(RolUser entity);
    }
}
