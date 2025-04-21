using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormRolPermissionCreateDTO
    {
        public int id { get; set; }
        public int id_forms { get; set; }
        public string Formsname { get; set; }
        public int id_rol { get; set; }

        public string RolName { get; set; }

        public int id_permission { get; set; }

        public string PermissionName { get; set; }
    }
}
