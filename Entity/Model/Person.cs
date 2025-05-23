using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Persons")]
    public class Person
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column("lastname")]
        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Column("document")]
        [StringLength(20)] // O el tamaño adecuado para tu tipo de documento
        // [Required] // Depende si el documento es siempre requerido
        public string Document { get; set; }

        [Column("phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        // Propiedad de Navegación inversa para la relación uno a uno (opcional, pero útil)
        // Si una Persona puede tener solo un User asociado (y viceversa)
        // EF Core puede inferir esto si User tiene PersonId y Person tiene una propiedad User.
        public virtual User User { get; set; }
    }
}