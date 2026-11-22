using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProyectoMotors.Modelos
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }

        public int IdCliente { get; set; }

        [Required, MaxLength(80)]
        public string Departamento { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Ciudad { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Zona { get; set; }

        [Required, MaxLength(120)]
        public string Calle { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Numero { get; set; }

        [MaxLength(200)]
        public string? Referencia { get; set; }

        public bool Activo { get; set; } = true;

        [ForeignKey(nameof(IdCliente)), JsonIgnore]
        public Cliente? Cliente { get; set; }
    }
}