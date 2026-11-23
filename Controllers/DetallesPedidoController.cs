using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/detalles-pedido")]
[SesionRequerida]
public class DetallesPedidoController : ControllerBase
{
    private readonly AppDbContext _db;
    public DetallesPedidoController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetallePedido>>> GetLista()
        => Ok(await _db.DetallesPedido.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DetallePedido>> GetPorId(int id)
    {
        var d = await _db.DetallesPedido.AsNoTracking().FirstOrDefaultAsync(x => x.IdDetalle == id);
        return d is null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<ActionResult<DetallePedido>> Post(DetallePedido detalle)
    {
        if (!await _db.Pedidos.AnyAsync(p => p.IdPedido == detalle.IdPedido))
            return BadRequest(new { mensaje = "El pedido indicado no existe." });

        if (detalle.Cantidad <= 0)
            return BadRequest(new { mensaje = "La cantidad debe ser mayor que cero." });

        detalle.IdDetalle = 0;
        detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
        detalle.Activo = true;

        _db.DetallesPedido.Add(detalle);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = detalle.IdDetalle }, detalle);
    }
}