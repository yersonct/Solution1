using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class CamaraDTO
{
    public int id { get; set; }
    public bool nightvisioninfrared { get; set; }
    public bool highresolution { get; set; }
    public bool infraredlighting { get; set; }
    public string name { get; set; }
    public bool optimizedangleofvision { get; set; }
    public bool highshutterspeed { get; set; }
}


}