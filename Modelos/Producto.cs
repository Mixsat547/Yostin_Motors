using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ProyectoMotors.Modelos
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        public int IdCategoria { get; set; }

        public int IdMarca { get; set; }

        [Required, MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Precision(12, 2)]
        public decimal Precio { get; set; }

        public bool Activo { get; set; } = true;

        [ForeignKey(nameof(IdCategoria)), JsonIgnore]
        public Categoria? Categoria { get; set; }

        [ForeignKey(nameof(IdMarca)), JsonIgnore]
        public Marca? Marca { get; set; }

        [JsonIgnore]
        public Inventario? Inventario { get; set; }

        [JsonIgnore]
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}
