using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolUser
    {
        public int id { get; set; }
        public int id_rol { get; set; }
        public int id_user { get; set; }
        public  User User { get; set; }
        public  Rol Rol { get; set; }
    }
}
