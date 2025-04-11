using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RatesDTO
    {
        public int id { get; set; }
        public double amount { get; set; }
        public TimeSpan startduration { get; set; }
        public TimeSpan endduration { get; set; }

        public int id_typerates { get; set; } // ← para persistencia
        public string typerates { get; set; } // ← para visualización (opcional)

    }
}
