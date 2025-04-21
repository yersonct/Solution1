using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<bool> UpdateInvoiceAsync(Invoice invoice);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
