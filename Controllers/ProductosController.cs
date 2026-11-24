using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/productos")]
[SesionRequerida]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetLista()
        => Ok(await _db.Productos.AsNoTracking().Where(x => x.Activo).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Producto>> GetPorId(int id)
    {
        var item = await _db.Productos.FirstOrDefaultAsync(x => x.IdProducto == id && x.Activo);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Producto>> Post(Producto item)
    {
        if (!await _db.Categorias.AnyAsync(x => x.IdCategoria == item.IdCategoria && x.Activo))
            return BadRequest(new { mensaje = "La categoría no existe o está inactiva." });

        if (!await _db.Marcas.AnyAsync(x => x.IdMarca == item.IdMarca && x.Activo))
            return BadRequest(new { mensaje = "La marca no existe o está inactiva." });

        item.IdProducto = 0;
        item.Activo = true;
        _db.Productos.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = item.IdProducto }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Producto datos)
    {
        var item = await _db.Productos.FirstOrDefaultAsync(x => x.IdProducto == id && x.Activo);
        if (item is null) return NotFound();

        if (!await _db.Categorias.AnyAsync(x => x.IdCategoria == datos.IdCategoria && x.Activo))
            return BadRequest(new { mensaje = "La categoría no existe o está inactiva." });

        if (!await _db.Marcas.AnyAsync(x => x.IdMarca == datos.IdMarca && x.Activo))
            return BadRequest(new { mensaje = "La marca no existe o está inactiva." });

        item.IdCategoria = datos.IdCategoria;
        item.IdMarca = datos.IdMarca;
        item.Nombre = datos.Nombre;
        item.Descripcion = datos.Descripcion;
        item.Precio = datos.Precio;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Productos.FirstOrDefaultAsync(x => x.IdProducto == id && x.Activo);
        if (item is null) return NotFound();

        item.Activo = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
