using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(User entity);
    }
}
