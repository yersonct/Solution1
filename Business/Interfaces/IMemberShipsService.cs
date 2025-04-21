using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMembershipsService
    {
        Task<IEnumerable<MemberShips>> GetAllMembershipsAsync();
        Task<MemberShips?> GetMembershipByIdAsync(int id);
        Task<MemberShips> CreateMembershipAsync(MemberShips membership);
        Task<bool> UpdateMembershipAsync(MemberShips membership);
        Task<bool> DeleteMembershipAsync(int id);
    }
}
