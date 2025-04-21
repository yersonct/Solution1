using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{


    public class User
    {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public int id_person { get; set; }

            public Person person { get; set; }
        
            public  List<RolUser> RolUsers { get; set; } = new List<RolUser>();


    }
}
