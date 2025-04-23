using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    [Table("client")]
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? id_user { get; set; }
        public bool active { get; set; } // Nueva propiedad para el eliminador lógico

        public User user { get; set; }
        public BlackList blacklist { get; set; }

        public List<Vehicle> vehicles { get; set; }
    }
}
