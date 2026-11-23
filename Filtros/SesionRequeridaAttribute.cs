using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoMotors.Filtros;

// Rechaza con 401 cualquier petición que no tenga una sesión iniciada
public class SesionRequeridaAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult(
                new { mensaje = "Debes iniciar sesión para usar este recurso." });
            return;
        }

        await next();
    }
}