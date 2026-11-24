using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProyectoMotors.Modelos
{
    public class Inventario
    {
        [Key]
        public int IdInventario { get; set; }

        public int IdProducto { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        [ForeignKey(nameof(IdProducto)), JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
