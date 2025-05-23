using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Rols")]
    public class Rol
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("description")]
        [StringLength(255)] // La longitud dependerá de tus necesidades
        public string Description { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        // Propiedades de Navegación
        public virtual ICollection<RolUser> RolUsers { get; set; } = new List<RolUser>();
        public virtual ICollection<FormRolPermission> FormRolPermissions { get; set; } = new List<FormRolPermission>();

        public Rol()
        {
            // Inicialización de colecciones ya en la declaración de la propiedad
        }
    }
}