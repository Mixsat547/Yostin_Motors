using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ProyectoMotors.Modelos
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public int IdDireccion { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(30)]
        public string Estado { get; set; } = "Pendiente";

        [Precision(12, 2)]
        public decimal Total { get; set; }

        public bool Activo { get; set; } = true;

        [ForeignKey(nameof(IdCliente)), JsonIgnore]
        public Cliente? Cliente { get; set; }

        [ForeignKey(nameof(IdDireccion)), JsonIgnore]
        public Direccion? Direccion { get; set; }

        [JsonIgnore]
        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}