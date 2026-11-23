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
Cómo contribuir

Lee CONTRIBUTING.md. Todos los aportes entran mediante Pull Request desde un fork, y ningún integrante es colaborador del repositorio base.