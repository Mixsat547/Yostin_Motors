using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoMotors.Modelos
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required, MaxLength(80)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Apellido { get; set; } = string.Empty;

        [Required, MaxLength(120)]
        public string Correo { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        // Usar siempre UtcNow: Npgsql guarda DateTime como "timestamp with time zone"
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        [JsonIgnore]
        public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();

        [JsonIgnore]
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}