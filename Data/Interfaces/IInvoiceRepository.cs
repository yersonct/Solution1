using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<Invoice?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Invoice entity);
    }
}
