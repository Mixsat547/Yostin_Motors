using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/marcas")]
[SesionRequerida]
public class MarcasController : ControllerBase
{
    private readonly AppDbContext _db;
    public MarcasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marca>>> GetLista()
        => Ok(await _db.Marcas.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Marca>> GetPorId(int id)
    {
        var item = await _db.Marcas.FirstOrDefaultAsync(x => x.IdMarca == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Marca>> Post(Marca item)
    {
        item.IdMarca = 0;
        item.Activo = true;
        _db.Marcas.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = item.IdMarca }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Marca datos)
    {
        var item = await _db.Marcas.FirstOrDefaultAsync(x => x.IdMarca == id);
        if (item is null) return NotFound();

        item.Nombre = datos.Nombre;
        item.Descripcion = datos.Descripcion;
        await _db.SaveChangesAsync();
        return NoContent();
    }

}
