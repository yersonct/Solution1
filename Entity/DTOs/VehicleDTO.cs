using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public int Id_Client { get; set; }

        public Client Client { get; set; }
        public RegisteredVehicle? RegisteredVehicle { get; set; }
        public MembershipsVehicle? MembershipsVehicle { get; set; }
    }
}
