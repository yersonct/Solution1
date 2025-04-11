using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RegisteredVehicleCreateDTO
    {
        public TimeSpan entrydatetime { get; set; } // CAMBIO: DateTime a TimeSpan
        public TimeSpan exitdatetime { get; set; }  // CAMBIO: DateTime a TimeSpan

        public int id_vehicle { get; set; }
    }
}