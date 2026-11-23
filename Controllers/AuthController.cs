using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoMotors.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpFactory;

    public AuthController(IConfiguration config, IHttpClientFactory httpFactory)
    {
        _config = config;
        _httpFactory = httpFactory;
    }

    // Crea la cookie de autenticación con la identidad del usuario
    private async Task IniciarSesion(string id, string nombre, string proveedor)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Name, nombre),
            new("provider", proveedor),
            new(ClaimTypes.Role, "Usuario")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));
    }

    // ---------------- DISCORD ----------------

    [HttpGet("discord/login")]
    public IActionResult DiscordLogin()
    {
        var clientId = _config["Discord:ClientId"];
        var redirectUri = _config["Discord:RedirectUri"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(redirectUri))
            return StatusCode(500, "Falta la configuración de Discord (user-secrets).");

        var state = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        HttpContext.Session.SetString("oauth_state", state);

        var url = "https://discord.com/oauth2/authorize"
                + $"?client_id={Uri.EscapeDataString(clientId)}"
                + $"&redirect_uri={Uri.EscapeDataString(redirectUri)}"
                + "&response_type=code"
                + "&scope=identify"
                + $"&state={state}";

        return Redirect(url);
    }

    [HttpGet("discord/callback")]
    public async Task<IActionResult> DiscordCallback(string? code, string? state, string? error)
    {
        if (!string.IsNullOrEmpty(error))
            return BadRequest($"Discord devolvió un error: {error}");

        var expectedState = HttpContext.Session.GetString("oauth_state");
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state) || state != expectedState)
            return BadRequest("Estado inválido o código ausente.");

        HttpContext.Session.Remove("oauth_state");

        var client = _httpFactory.CreateClient();

        var tokenResponse = await client.PostAsync("https://discord.com/api/oauth2/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _config["Discord:ClientId"]!,
                ["client_secret"] = _config["Discord:ClientSecret"]!,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = _config["Discord:RedirectUri"]!
            }));

        if (!tokenResponse.IsSuccessStatusCode)
            return StatusCode(502, "No se pudo obtener el token de Discord.");

        using var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
        var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var userResponse = await client.SendAsync(request);

        if (!userResponse.IsSuccessStatusCode)
            return StatusCode(502, "No se pudo obtener el usuario de Discord.");

        using var userJson = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync());
        var id = userJson.RootElement.GetProperty("id").GetString();
        var username = userJson.RootElement.GetProperty("username").GetString();

        await IniciarSesion(id!, username!, "Discord");

        return Redirect("/");
    }

    // ---------------- GOOGLE ----------------

    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var clientId = _config["Google:ClientId"];
        var redirectUri = _config["Google:RedirectUri"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(redirectUri))
            return StatusCode(500, "Falta la configuración de Google (user-secrets).");

        var state = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        HttpContext.Session.SetString("oauth_state", state);

        var url = "https://accounts.google.com/o/oauth2/v2/auth"
                + $"?client_id={Uri.EscapeDataString(clientId)}"
                + $"&redirect_uri={Uri.EscapeDataString(redirectUri)}"
                + "&response_type=code"
                + "&scope=" + Uri.EscapeDataString("openid profile email")
                + $"&state={state}";

        return Redirect(url);
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback(string? code, string? state, string? error)
    {
        if (!string.IsNullOrEmpty(error))
            return BadRequest($"Google devolvió un error: {error}");

        var expectedState = HttpContext.Session.GetString("oauth_state");
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state) || state != expectedState)
            return BadRequest("Estado inválido o código ausente.");

        HttpContext.Session.Remove("oauth_state");

        var client = _httpFactory.CreateClient();

        var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _config["Google:ClientId"]!,
                ["client_secret"] = _config["Google:ClientSecret"]!,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = _config["Google:RedirectUri"]!
            }));

        if (!tokenResponse.IsSuccessStatusCode)
            return StatusCode(502, "No se pudo obtener el token de Google.");

        using var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
        var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var userResponse = await client.SendAsync(request);

        if (!userResponse.IsSuccessStatusCode)
            return StatusCode(502, "No se pudo obtener el usuario de Google.");

        using var userJson = JsonDocument.Parse(await userResponse.Content.ReadAsStringAsync());
        var id = userJson.RootElement.GetProperty("sub").GetString();
        var name = userJson.RootElement.TryGetProperty("name", out var n) ? n.GetString() : "Usuario Google";

        await IniciarSesion(id!, name ?? "Usuario Google", "Google");

        return Redirect("/");
    }

    // ---------------- SESIÓN ----------------

    [HttpGet("me")]
    public IActionResult Me()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized(new { mensaje = "No hay sesión iniciada." });

        return Ok(new
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            userName = User.Identity!.Name,
            provider = User.FindFirstValue("provider"),
            rol = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { mensaje = "Sesión cerrada." });
    }
}