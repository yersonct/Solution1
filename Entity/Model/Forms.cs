using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Forms
    {

        public int Id { get; set; }
        public string Name { get; set; } 
        public int ModuloId { get; set; }
        public string Url { get; set; }
        public List<FormModule> FormModules { get; set; } = new List<FormModule>();
        public List<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();


    }
}
