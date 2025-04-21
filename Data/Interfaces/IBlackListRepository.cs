using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBlacklistRepository : IRepository<BlackList>
    {
        Task AddBlacklistAsync(BlackList entity);
        //Task<IEnumerable<BlackList>> GetAllWithClientAsync();
        //Task<BlackList> GetByIdWithClientAsync(int id);

    }
}
