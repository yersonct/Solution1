using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class VehicleHistoryDTO
    {
        public int id { get; set; }
        public TimeSpan TotalTime { get; set; }

        public int RegisteredVehicleId { get; set; }
        //public RegisteredVehicle RegisteredVehicle { get; set; }

        public int TypeVehicleId { get; set; }
        //public TypeVehicle TypeVehicle { get; set; }

        // Si aplica
        //public int? InvoiceId { get; set; }
        //public Invoice Invoice { get; set; }
    }

}
