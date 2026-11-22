# Guía de contribución

Todo aporte se envía mediante una Pull Request (PR) al repositorio del líder. Nadie hace push directo a `main` y ningún integrante es colaborador del repositorio base.

## 1. Hacer el fork

1. Entra al repositorio base en GitHub y pulsa **Fork**.
2. Clona **tu fork**:
   ```bash
   git clone git@github.com:Wakas4/Yostin_Motors.git
   cd Nombre_del_proyecto
   ```
3. Agrega el repositorio del líder como `upstream` y mantén tu `main` al día:
   ```bash
   git remote add upstream git@github.com:Wakas4/Yostin_Motors.git
   git fetch upstream
   git switch main
   git merge upstream/main
   ```

## 2. Nombrar las ramas

Crea una rama nueva desde `main` actualizado para cada aporte, con el formato `tipo/descripcion-corta`:

- `feat/tablas-clientes`
- `feat/login-discord-google`
- `feat/controladores-post-get`
- `feat/put-soft-delete`
- `fix/ruta-duplicada`

## 3. Nombrar los commits

Formato: `tipo: descripción en presente y en minúscula`.

- `feat:` funcionalidad nueva
- `fix:` corrección
- `docs:` documentación
- `chore:` configuración o tareas menores

Ejemplos: `feat: agrega modelo y migración de Pedidos`, `docs: actualiza reparto de tablas en README`.

## 4. Probar los cambios

Antes de abrir la PR comprueba que:

1. `dotnet build` termina sin errores.
2. `dotnet ef database update` aplica las migraciones.
3. Los endpoints que tocaste responden bien en Scalar, con y sin sesión iniciada.
4. No subes credenciales ni `appsettings.json` (solo `appsettings.Example.json`).

## 5. Abrir la Pull Request

1. Sube tu rama: `git push -u origin nombre-de-tu-rama`
2. En GitHub, abre una PR desde tu fork hacia `main` del repositorio del líder.
3. Escribe un título claro y una descripción con: qué cambia, qué tablas o endpoints toca y cómo lo probaste.

## 6. Solicitar y atender la revisión

1. Pide revisión al líder en **Reviewers** (o menciónalo en un comentario).
2. Responde los comentarios con nuevos commits en la misma rama.
3. Solo el líder integra la PR. La última PR (PUT y soft delete) se integra con **Squash and merge**.

En las PR que abre el líder, otro integrante registra la revisión y el líder las integra.

## Reglas generales

- Una PR por etapa, con cambios acotados.
- Las PR duplicadas o con conflictos de rutas se rechazan y se cierran sin integrar, conservando los comentarios.
- Las credenciales reales nunca se suben al repositorio.