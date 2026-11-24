using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/categorias")]
[SesionRequerida]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetLista()
        => Ok(await _db.Categorias.AsNoTracking().Where(x => x.Activo).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Categoria>> GetPorId(int id)
    {
        var item = await _db.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id && x.Activo);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Post(Categoria item)
    {
        item.IdCategoria = 0;
        item.Activo = true;
        _db.Categorias.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = item.IdCategoria }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Categoria datos)
    {
        var item = await _db.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id && x.Activo);
        if (item is null) return NotFound();

        item.Nombre = datos.Nombre;
        item.Descripcion = datos.Descripcion;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id && x.Activo);
        if (item is null) return NotFound();

        item.Activo = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
