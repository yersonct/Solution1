using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? id_user { get; set; } // Opcional, si aún quieres el ID
        public string UserName { get; set; } // Nueva propiedad para el nombre de usuario
        //public int BlackListId { get; set; }
        //public List<int> VehicleId { get; set; }
    }
}
