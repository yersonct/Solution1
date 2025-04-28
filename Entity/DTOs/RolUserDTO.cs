using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolUserDTO
    {
        public int id { get; set; }
        public int id_rol { get; set; }

        public int id_user { get; set; }
        public string RolName { get; set; }
        public string UserName { get; set; }
        public bool active { get; set; }
    }
}