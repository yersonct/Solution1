using Entity.Context; // Asegúrate de ajustar el namespace
using Entity.Model;  // Asegúrate de ajustar el namespace
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;

namespace Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
    }
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context; // Usa tu DbContext

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.username == username);
        }
    }
}