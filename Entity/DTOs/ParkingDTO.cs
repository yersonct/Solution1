using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ParkingDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string hability { get; set; }
        public int id_camara { get; set; }
    }
}
