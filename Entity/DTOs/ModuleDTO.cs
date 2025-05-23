// Entity/DTOs/ModuleDTO.cs (Para respuestas de lectura)

using System;
using System.Collections.Generic;

namespace Entity.DTOs
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
