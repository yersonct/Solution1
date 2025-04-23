using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class MembershipsVehicle
    {
        public int id { get; set; }

        public int id_vehicle { get; set; }
        public Vehicle vehicle { get; set; }

        public int id_memberships { get; set; }
        public MemberShips memberships { get; set; }

        public bool active { get; set; } // Para eliminación lógica
    }
}
