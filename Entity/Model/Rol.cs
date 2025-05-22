using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Rol
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public List<RolUser> RolUsers { get; set; } = new List<RolUser>();
        public List<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();
    }
}
