using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        void Update(T entity);
    }
}
