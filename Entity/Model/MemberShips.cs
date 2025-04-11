using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class MemberShips
    {
        public int id { get; set; }
        public string membershiptype { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public bool active { get; set; }

        public List<MembershipsVehicle> membershipsvehicles { get; set; } = new List<MembershipsVehicle>();

    }
}
