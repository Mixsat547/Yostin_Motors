using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoMotors.Modelos
{
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }

        [Required, MaxLength(80)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        [JsonIgnore]
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
