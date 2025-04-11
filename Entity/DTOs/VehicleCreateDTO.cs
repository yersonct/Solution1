using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class VehicleCreateDTO
    {
        public string plate { get; set; }
        public string color { get; set; }
        public int id_client { get; set; }
    }
}
