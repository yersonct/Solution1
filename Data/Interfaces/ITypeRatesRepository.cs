using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITypeRatesRepository
    {
        Task<TypeRates> AddAsync(TypeRates entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TypeRates>> GetAllAsync();
        Task<TypeRates?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(TypeRates entity);
    }

}
