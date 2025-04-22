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
        public int? id_user { get; set; }
        //public int BlackListId { get; set; }
        //public List<int> VehicleId { get; set; }
    }
}
