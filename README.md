Yostin Motors API

API web en .NET 10 para la gestión de una tienda de motocicletas: clientes, direcciones, pedidos, productos e inventario. Los usuarios inician sesión con proveedores externos (OAuth 2) y los endpoints están protegidos desde el backend.

Tecnologías
.NET 10 (Web API con controladores)
Entity Framework Core + Npgsql (PostgreSQL)
Scalar para probar los endpoints
OAuth 2 con sesión de servidor (Discord, Google y los proveedores del otro integrante)
IMPORTANTE

Para evitar conflictos es necesario instalar las versiones correctas de los Paquetes

Paquetes NuGet con versión fija para .NET 10: Microsoft.EntityFrameworkCore 10.0.3, Microsoft.EntityFrameworkCore.Design 10.0.3 y Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0 (instalar siempre con --version)
Equipo y reparto
Integrante	Rol	GitHub	Tablas	Proveedores de login
Yeltsin Sardina	Líder	@Wakas4	Clientes, Direcciones, Pedidos, DetallesPedido	Discord (fácil) + Google (media)
Misael Choque	Integrante	@Mixsat547	Productos, Categorias, Marcas, Inventario	Twitch (fácil) + Microsoft (media)

Relaciones entre tablas: Direcciones y Pedidos dependen de Clientes; DetallesPedido depende de Pedidos y de Productos; Productos depende de Categorias y Marcas; Inventario depende de Productos.

Requisitos
.NET SDK 10
PostgreSQL
Herramienta dotnet-ef: dotnet tool install --global dotnet-ef
Ejecución local
Clona el repositorio (o tu fork) y entra a la carpeta del proyecto.
Copia appsettings.Example.json como appsettings.json y ajusta la cadena de conexión.
Guarda las credenciales de los proveedores fuera del repositorio, con user-secrets o variables de entorno (consulta appsettings.Example.json para ver los nombres de cada clave). Con user-secrets: dotnet user-secrets set "Discord:ClientId" "...", y lo mismo con Discord:ClientSecret, Discord:RedirectUri (https://localhost:7025/api/auth/discord/callback), Google:ClientId, Google:ClientSecret y Google:RedirectUri (https://localhost:7025/api/auth/google/callback).
Aplica las migraciones: dotnet ef database update
Ejecuta con el perfil HTTPS, que el login necesita: dotnet run --launch-profile https
Abre Scalar en https://localhost:7025/scalar. Para probar los endpoints, inicia sesión antes en /api/auth/discord/login o /api/auth/google/login, en la misma ventana del navegador. Sin sesión los endpoints responden 401. Para cerrar sesión usa /api/auth/logout.

## Imagen en Docker Hub

- Repositorio: https://hub.docker.com/r/yeltsinsardina/yostinmotors
- Referencia: `yeltsinsardina/yostinmotors:1.0.0`
- Descargar: `docker pull yeltsinsardina/yostinmotors:1.0.0`
- Se construye con `Dockerfile.YeltsinSardina` (proyecto `ProyectoMotors.csproj`):
  `docker build -f Dockerfile.YeltsinSardina -t yeltsinsardina/yostinmotors:1.0.0 .`
- La imagen no incluye credenciales: se pasan como variables de entorno (ver `.env.example`).

## Release con cuatro instancias

Usa la imagen publicada en Docker Hub, sin compilar el código.

1. Copia `.env.example` como `.env` y completa los valores, incluida `API_IMAGE` (`yeltsinsardina/yostinmotors:1.0.0`).
2. Registra en Discord y Google las URL de retorno de los puertos 5001 a 5004 (`http://localhost:500X/api/auth/discord/callback` y `.../google/callback`).
3. `docker compose -f docker-compose.release.yml pull`
4. `docker compose -f docker-compose.release.yml up -d`
5. Las instancias quedan en `http://localhost:5001`, `5002`, `5003` y `5004`. La primera crea las tablas al iniciar.
6. Inicia sesión en un puerto (`/api/auth/discord/login`) y comprueba `/api/auth/me` y `/api/reportes/clientes-pedidos` en los demás. Las cuatro comparten la cookie mediante el volumen `keys`.

Cómo contribuir

Lee CONTRIBUTING.md. Todos los aportes entran mediante Pull Request desde un fork, y ningún integrante es colaborador del repositorio base.