using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class MembershipsVehicleDTO
    {
        public int id { get; set; }
        public int id_vehicle { get; set; }
        public string VehiclePlate { get; set; }
        public int id_memberships { get; set; }
        public bool active { get; set; }
        public string MembershipsNmae { get; set; }
    }
}
