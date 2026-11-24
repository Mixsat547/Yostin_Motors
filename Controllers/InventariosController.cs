using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/inventarios")]
[SesionRequerida]
public class InventariosController : ControllerBase
{
    private readonly AppDbContext _db;
    public InventariosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inventario>>> GetLista()
        => Ok(await _db.Inventarios.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Inventario>> GetPorId(int id)
    {
        var item = await _db.Inventarios.FirstOrDefaultAsync(x => x.IdInventario == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Inventario>> Post(Inventario item)
    {
        if (!await _db.Productos.AnyAsync(x => x.IdProducto == item.IdProducto))
            return BadRequest(new { mensaje = "El producto no existe o está inactivo." });

        if (await _db.Inventarios.AnyAsync(x => x.IdProducto == item.IdProducto))
            return Conflict(new { mensaje = "Este producto ya tiene un inventario registrado." });

        item.IdInventario = 0;
        item.FechaActualizacion = DateTime.UtcNow;
        item.Activo = true;
        _db.Inventarios.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = item.IdInventario }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Inventario datos)
    {
        var item = await _db.Inventarios.FirstOrDefaultAsync(x => x.IdInventario == id);
        if (item is null) return NotFound();

        if (!await _db.Productos.AnyAsync(x => x.IdProducto == datos.IdProducto))
            return BadRequest(new { mensaje = "El producto no existe o está inactivo." });

        if (await _db.Inventarios.AnyAsync(x => x.IdProducto == datos.IdProducto && x.IdInventario != id))
            return Conflict(new { mensaje = "Este producto ya tiene otro inventario registrado." });

        item.IdProducto = datos.IdProducto;
        item.Stock = datos.Stock;
        item.StockMinimo = datos.StockMinimo;
        item.FechaActualizacion = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

}
