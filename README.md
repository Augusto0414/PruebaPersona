# Prueba técnica .NET – CRUD de Persona (API)

API ASP.NET Core en .NET 8 con SQLite (sin servidor de base de datos) y documentación Swagger.

## Estructura

```
PruebaPersona/
├── PruebaPersona.sln               # Solución de Visual Studio
├── PruebaPersona.csproj
├── Program.cs                      # Endpoints de la API
├── Properties/launchSettings.json  # Puerto de ejecución (5080)
├── Models/Persona.cs               # Entidad Persona
├── Models/EdadDto.cs               # Cuerpo para actualizar la edad
├── Data/Database.cs                # Conexión y creación de tabla y vista
├── Services/IPersonaService.cs     # Contrato del servicio
├── Services/PersonaService.cs      # Implementación del CRUD
├── persona.db                      # Base de datos con datos de ejemplo
└── Scripts/
    ├── schema.sql                  # Script de referencia (tabla, vista y pruebas)
    └── export_persona.sql          # Exportación de la base de datos
```

## Modelo de datos

Tabla `Persona`: `Identificacion` (PK), `Nombres`, `Apellidos`, `Edad`, `Genero` (`Masculino` / `Femenino`).

Vista `vw_PersonasFemeninas`: filtra `Genero = 'Femenino'`. Es la fuente del listado de mujeres.

## Endpoints

| Operación | Método HTTP | Ruta | Respuesta |
|---|---|---|---|
| Insertar | `POST` | `/api/personas` | 201 Created / 400 / 409 |
| Buscar por identificación | `GET` | `/api/personas/{identificacion}` | 200 / 404 |
| Listar mujeres (vista) | `GET` | `/api/personas/mujeres` | 200 |
| Actualizar edad | `PATCH` | `/api/personas/{identificacion}/edad` | 204 / 400 / 404 |
| Eliminar por identificación | `DELETE` | `/api/personas/{identificacion}` | 204 / 404 |

## Ejecutar

Desde la carpeta del proyecto:

```bash
dotnet restore
dotnet run
```

Luego abre **http://localhost:5080/swagger** (en Codespaces, usa la pestaña **Ports** y abre el puerto 5080 en el navegador).

## Probar con curl

```bash
# Insertar
curl -X POST http://localhost:5080/api/personas -H "Content-Type: application/json" \
  -d '{"identificacion":"2001","nombres":"Sofía","apellidos":"López","edad":30,"genero":"Femenino"}'

# Buscar
curl http://localhost:5080/api/personas/2001

# Listar mujeres (vista)
curl http://localhost:5080/api/personas/mujeres

# Actualizar edad
curl -X PATCH http://localhost:5080/api/personas/2001/edad -H "Content-Type: application/json" -d '{"edad":31}'

# Eliminar
curl -X DELETE http://localhost:5080/api/personas/2001
```

## Notas

- La base de datos `persona.db` se busca en la carpeta desde la que se ejecuta la app. Con `dotnet run` desde la carpeta del proyecto, usa el archivo incluido. Si se ejecuta desde Visual Studio con F5, la app crea una base nueva vacía en `bin/Debug/net8.0/`.
- Los datos de ejemplo ya vienen en `persona.db`. Si la base no existe, la app crea la tabla y la vista automáticamente.
