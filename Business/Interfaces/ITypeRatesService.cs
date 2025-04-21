using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITypeRatesService
    {
        Task<IEnumerable<TypeRates>> GetAllTypeRatesAsync();
        Task<TypeRates?> GetTypeRatesByIdAsync(int id);
        Task<TypeRates> CreateTypeRatesAsync(TypeRates typeRates);
        Task<bool> UpdateTypeRatesAsync(TypeRates typeRates);
        Task<bool> DeleteTypeRatesAsync(int id);
    }

}