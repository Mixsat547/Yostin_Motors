using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoMotors.Data;

namespace ProyectoMotors.Controllers;

/// Reporte: Resumen de clientes y pedidos
/// Autor: Yeltsin Sardina (@Wakas4)
/// Tablas: Clientes, Direcciones, Pedidos, DetallesPedido
/// Propósito: ver por cliente su ciudad, cuántos pedidos activos tiene,
///            cuántos artículos compró y cuánto gastó en total.
/// Acceso: solo el rol Administrador.
[ApiController]
[Route("api/reportes")]
[Authorize(Roles = "Administrador")]
public class ReportesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ReportesController(AppDbContext db) => _db = db;

    [HttpGet("clientes-pedidos")]
    public async Task<IActionResult> ClientesPedidos()
    {
        var datos = await _db.Clientes
            .AsNoTracking()
            .Where(c => c.Activo)
            .Select(c => new
            {
                c.IdCliente,
                Cliente = c.Nombre + " " + c.Apellido,
                c.Correo,
                Ciudad = c.Direcciones.Where(d => d.Activo).Select(d => d.Ciudad).FirstOrDefault(),
                TotalPedidos = c.Pedidos.Count(p => p.Activo),
                Articulos = c.Pedidos.Where(p => p.Activo)
                                .SelectMany(p => p.Detalles)
                                .Where(d => d.Activo)
                                .Sum(d => (int?)d.Cantidad) ?? 0,
                MontoTotal = c.Pedidos.Where(p => p.Activo)
                                .Sum(p => (decimal?)p.Total) ?? 0
            })
            .OrderByDescending(x => x.MontoTotal)
            .ToListAsync();

        return Ok(datos);
    }
}