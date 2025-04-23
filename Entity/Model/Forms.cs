using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Forms
    {

        public int id { get; set; }
        public string name { get; set; } 
        public string url { get; set; }

        public bool active { get; set; }
        public List<FormModule> FormModules { get; set; } = new List<FormModule>();
        public List<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();


    }
}
