using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolUserRepository
    {
        Task<IEnumerable<Entity.Model.RolUser>> GetAllAsync();
        Task<Entity.Model.RolUser?> GetByIdAsync(int id);
        Task<Entity.Model.RolUser> AddAsync(Entity.Model.RolUser entity);
        Task<bool> UpdateAsync(Entity.Model.RolUser entity); // Recibe la entidad para actualizar
        Task<bool> DeleteAsync(int id); // O si es borrado lógico, que reciba la entidad o el ID
    }
}
