using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("RolUsers")]
    public class RolUser
    {
        // Nota: Si esta es una tabla de unión puramente para una relación Many-to-Many
        // la clave primaria a menudo es la combinación de las claves foráneas (RolId, UserId).
        // Sin embargo, si necesita propiedades adicionales o un ID propio, lo mantienes.
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // Claves foráneas
        [Column("id_rol")]
        public int RolId { get; set; } // Cambiado de id_rol a RolId

        [Column("id_user")]
        public int UserId { get; set; } // Cambiado de id_user a UserId

        [Column("active")]
        public bool Active { get; set; }

        // Propiedades de Navegación
        public virtual User User { get; set; }
        public virtual Rol Rol { get; set; }
    }
}