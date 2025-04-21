using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
   public class Camara
    {
        public int id { get; set; }
        public string? name { get; set; }= null;
        public bool nightvisioninfrared { get; set; }
        public bool highresolution { get; set; }
        public bool infraredlighting { get; set; }
        public bool optimizedangleofvision { get; set; }
        public bool highshutterspeed { get; set; }

        // Relación con Parking (uno a muchos)
        public Parking parking { get; set; }
    }


}
