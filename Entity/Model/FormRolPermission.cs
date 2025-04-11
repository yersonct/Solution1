using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
   public class FormRolPermission
{
    public int Id { get; set; }
    public int id_forms { get; set; }
    public int id_role { get; set; }
    public int id_permission { get; set; }
    public Rol Rol { get; set; }
    public Forms Forms { get; set; }
    public Permission Permission { get; set; }
}

}
