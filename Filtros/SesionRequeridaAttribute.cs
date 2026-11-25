using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoMotors.Filtros;

public class SesionRequeridaAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var usuario = context.HttpContext.User;
        var userId = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (usuario.Identity?.IsAuthenticated != true ||
            string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult(
                new { mensaje = "Debes iniciar sesión para usar este recurso." });
            return;
        }

        await next();
    }
}
