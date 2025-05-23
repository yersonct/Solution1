using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("FormModules")]
    public class FormModule
    {
        // Si 'id' es la clave primaria independiente para esta tabla intermedia
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // Claves Foráneas
        [Column("id_forms")]
        public int FormId { get; set; } // Cambiado de id_forms a FormId

        [Column("id_module")]
        public int ModuleId { get; set; } // Cambiado de id_module a ModuleId

        [Column("active")]
        public bool Active { get; set; }

        // Propiedades de Navegación
        public virtual Forms Forms { get; set; } // Renombrado de Forms a Form para consistencia
        public virtual Modules Modules { get; set; } // Renombrado de Modules a Module para consistencia
    }
}