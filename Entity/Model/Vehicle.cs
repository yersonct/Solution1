using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Vehicle
    {
        public int id { get; set; }
        public string plate { get; set; }
        public string color { get; set; }

        public int id_client { get; set; } // ❗ FK hacia Client
        public Client client { get; set; }

        public List<RegisteredVehicle> registeredvehicles { get; set; } = new List<RegisteredVehicle>();

        public List<MembershipsVehicle> membershipsvehicles { get; set; } = new List<MembershipsVehicle>();

    }
}
