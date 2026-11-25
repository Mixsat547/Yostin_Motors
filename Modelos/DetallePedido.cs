using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ProyectoMotors.Modelos
{
    public class DetallePedido
    {
        [Key]
        public int IdDetalle { get; set; }

        public int IdPedido { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        [Precision(12, 2)]
        public decimal PrecioUnitario { get; set; }

        [Precision(12, 2)]
        public decimal Subtotal { get; set; }

        public bool Activo { get; set; } = true;

        [ForeignKey(nameof(IdPedido)), JsonIgnore]
        public Pedido? Pedido { get; set; }

        [ForeignKey(nameof(IdProducto)), JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
