using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> AddAsync(Client entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Client entity);
    }

    // Métodos específicos de Client (si los necesitas)
    // Task<IEnumerable<Client>> GetClientsByNameAsync(string name);

}
