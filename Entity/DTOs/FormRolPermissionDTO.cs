using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormRolPermissionDTO
    {
        public int id { get; set; }
        public int id_forms { get; set; }

        public int id_rol { get; set; }

        public int id_permission { get; set; }

    }
}
