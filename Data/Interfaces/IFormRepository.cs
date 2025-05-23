// Data/Interfaces/IFormRepository.cs

using Entity.Model; // Asegúrate de que sea 'Form' singular
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFormRepository
    {
        Task<Forms> AddAsync(Forms entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Forms>> GetAllAsync();
        Task<Forms?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Forms entity);
    }
}