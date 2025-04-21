using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatesService
    {
        Task<IEnumerable<Rates>> GetAllRatesAsync();
        Task<Rates?> GetRatesByIdAsync(int id);
        Task<Rates> CreateRatesAsync(Rates rates);
        Task<bool> UpdateRatesAsync(Rates rates);
        Task<bool> DeleteRatesAsync(int id);
    }
}
