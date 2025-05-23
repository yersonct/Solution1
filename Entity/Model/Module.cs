using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Modules")] // Cambiado de 'Modules' a 'Module' para seguir la convención de singular para el nombre de la clase
    public class Modules // Considera renombrar esta clase a 'Module' para coherencia de singular/plural
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        public virtual ICollection<FormModule> FormModules { get; set; } = new List<FormModule>();

        public Modules()
        {
            // Inicialización de colecciones
        }
    }
}