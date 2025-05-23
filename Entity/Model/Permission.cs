// Entity/Model/Permission.cs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        public virtual ICollection<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();

        public Permission()
        {
            // Colecciones ya inicializadas en la declaración de la propiedad
        }
    }
}