using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string document { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool active { get; set; }

        public User user { get; set; }
    }
}
