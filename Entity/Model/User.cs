using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para Data Annotations como [Key]
using System.ComponentModel.DataAnnotations.Schema; // Para [Table], [Column]

namespace Entity.Model
{
    [Table("Users")] // Mapea la clase User a una tabla llamada "Users"
    public class User
    {
        // Clave Primaria
        [Key] // Marca explícitamente 'Id' como la clave primaria (aunque EF Core la infiere por convención)
        [Column("id")] // Mapea la propiedad 'Id' a una columna llamada 'id' en la BD
        public int Id { get; set; }

        [Column("username")]
        [Required] // Hace que la columna sea NOT NULL en la BD
        [StringLength(50)] // Limita la longitud en la BD
        public string Username { get; set; }

        [Column("password")]
        [Required]
        // [StringLength(255)] // Asumiendo que almacenarás un hash de contraseña
        public string Password { get; set; } // Nota: Considera renombrar a PasswordHash si almacenas el hash

        // Clave Foránea a Person
        [Column("id_person")]
        public int PersonId { get; set; } // Cambiado de id_person a PersonId por convención de EF Core

        [Column("active")]
        public bool Active { get; set; }

        // Propiedad de Navegación a Person (relación uno-a-uno o uno-a-muchos)
        // EF Core inferirá la relación usando PersonId
        public virtual Person Person { get; set; }

        // Propiedad de Navegación para la tabla intermedia RolUser (relación muchos a muchos)
        public virtual ICollection<RolUser> RolUsers { get; set; } = new List<RolUser>();

        // Constructor para asegurar que las colecciones no sean nulas
        public User()
        {
            // Ya está inicializado en la declaración de la propiedad, pero puedes hacerlo aquí también
            // RolUsers = new List<RolUser>();
        }
    }
}