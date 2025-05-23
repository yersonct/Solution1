    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Entity.Model
    {
        [Table("Forms")]
        public class Forms
        {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Column("name")]
            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [Column("url")]
            [StringLength(255)] // Longitud para una URL
            public string Url { get; set; } // Considera renombrar a URL o Ruta

            [Column("active")]
            public bool Active { get; set; }

            public virtual ICollection<FormModule> FormModules { get; set; } = new List<FormModule>();
            public virtual ICollection<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();

            public Forms()
            {
                // Inicialización de colecciones
            }
        }
    }