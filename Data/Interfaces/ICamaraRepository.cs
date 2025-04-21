using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICamaraRepository
    {
        Task<IEnumerable<Camara>> GetAllAsync();
        Task<Camara?> GetByIdAsync(int id);
        Task<Camara> CreateAsync(Camara entity);
        Task<bool> UpdateAsync(Camara entity);
        Task<bool> DeleteAsync(int id);
    }
}
