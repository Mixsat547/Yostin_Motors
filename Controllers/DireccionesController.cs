using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;
using ProyectoMotors.Filtros;
using ProyectoMotors.Modelos;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/direcciones")]
[SesionRequerida]
public class DireccionesController : ControllerBase
{
    private readonly AppDbContext _db;
    public DireccionesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Direccion>>> GetLista()
        => Ok(await _db.Direcciones.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Direccion>> GetPorId(int id)
    {
        var d = await _db.Direcciones.AsNoTracking().FirstOrDefaultAsync(x => x.IdDireccion == id);
        return d is null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<ActionResult<Direccion>> Post(Direccion direccion)
    {
        if (!await _db.Clientes.AnyAsync(c => c.IdCliente == direccion.IdCliente))
            return BadRequest(new { mensaje = "El cliente indicado no existe." });

        direccion.IdDireccion = 0;
        direccion.Activo = true;

        _db.Direcciones.Add(direccion);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPorId), new { id = direccion.IdDireccion }, direccion);
    }
}