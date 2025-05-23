using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("FormRolPermissions")]
    public class FormRolPermission
    {
        // Si 'id' es la clave primaria independiente para esta tabla intermedia
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // Claves Foráneas
        [Column("id_forms")]
        public int FormId { get; set; } // Cambiado de id_forms a FormId

        [Column("id_rol")]
        public int RolId { get; set; } // Cambiado de id_rol a RolId

        [Column("id_permission")]
        public int PermissionId { get; set; } // Cambiado de id_permission a PermissionId

        // Propiedades de Navegación
        public virtual Rol Rol { get; set; }
        public virtual Forms Forms { get; set; } // Renombrado de Forms a Form para consistencia
        public virtual Permission Permission { get; set; }

        [Column("active")]
        public bool Active { get; set; }
    }
}