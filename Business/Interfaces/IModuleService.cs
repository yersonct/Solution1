using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<Modules>> GetAllModulesAsync();
        Task<Modules?> GetModuleByIdAsync(int id);
        Task<Modules> CreateModuleAsync(Modules module);
        Task<bool> UpdateModuleAsync(Modules module);
        Task<bool> DeleteModuleAsync(int id);
    }
}
