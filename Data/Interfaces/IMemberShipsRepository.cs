using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMembershipsRepository
    {
        Task<MemberShips> AddAsync(MemberShips entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<MemberShips>> GetAllAsync();
        Task<MemberShips?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(MemberShips entity);
    }
}
