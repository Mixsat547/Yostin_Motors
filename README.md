# Yostin Motors API

API web en .NET 10 para la gestión de una tienda de motocicletas: clientes, direcciones, pedidos, productos e inventario. Los usuarios inician sesión con proveedores externos (OAuth 2) y los endpoints están protegidos desde el backend.

## Tecnologías

- .NET 10 (Web API con controladores)
- Entity Framework Core + Npgsql (PostgreSQL)
- Scalar para probar los endpoints
- OAuth 2 con sesión de servidor (Discord, Google y los proveedores del otro integrante)

## IMPORTANTE
Para evitar conflictos es necesario instalar las versiones correctas de los Paquetes
- Paquetes NuGet con versión fija para .NET 10: 
Microsoft.EntityFrameworkCore 10.0.3, Microsoft.EntityFrameworkCore.Design 10.0.3 y Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0 (instalar siempre con --version)

## Equipo y reparto

| Integrante | Rol | GitHub | Tablas | Proveedores de login |
|---|---|---|---|---|
| Yeltsin Sardina | Líder | @Wakas4 | Clientes, Direcciones, Pedidos, DetallesPedido | Discord (fácil) + Google (media) |
| NOMBRE DEL INTEGRANTE | Integrante | @USUARIO_INTEGRANTE | Productos, Categorias, Marcas, Inventario | Twitch FÁCIL + Microsoft MEDIO/DIFÍCIL |

Relaciones entre tablas: `Direcciones` y `Pedidos` dependen de `Clientes`; `DetallesPedido` depende de `Pedidos` y de `Productos`; `Productos` depende de `Categorias` y `Marcas`; `Inventario` depende de `Productos`.

## Requisitos

- .NET SDK 10
- PostgreSQL
- Herramienta `dotnet-ef`: `dotnet tool install --global dotnet-ef`

## Ejecución local

1. Clona el repositorio (o tu fork) y entra a la carpeta del proyecto.
2. Copia `appsettings.Example.json` como `appsettings.json` y ajusta la cadena de conexión.
3. Guarda las credenciales de los proveedores fuera del repositorio, con *user-secrets* o variables de entorno (consulta `appsettings.Example.json` para ver los nombres de cada clave).
4. Aplica las migraciones: `dotnet ef database update`
5. Ejecuta: `dotnet run`
6. Abre Scalar en la URL que indique la consola, en la ruta `/scalar`.

## Cómo contribuir

Lee [CONTRIBUTING.md](CONTRIBUTING.md). Todos los aportes entran mediante Pull Request desde un fork, y ningún integrante es colaborador del repositorio base.