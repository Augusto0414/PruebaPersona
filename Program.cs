using Microsoft.Data.Sqlite;
using PruebaPersona.Data;
using PruebaPersona.Models;
using PruebaPersona.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPersonaService, PersonaService>();

var app = builder.Build();

// Crea la tabla Persona y la vista vw_PersonasFemeninas si no existen.
Database.Inicializar();

app.UseSwagger();
app.UseSwaggerUI();

var personas = app.MapGroup("/api/personas").WithTags("Personas");

// INSERT
personas.MapPost("/", (Persona persona, IPersonaService servicio) =>
    Ejecutar(() =>
    {
        servicio.Insertar(persona);
        return Results.Created($"/api/personas/{persona.Identificacion}", persona);
    }))
    .WithSummary("Insertar una persona");

// SELECT desde la vista (solo mujeres). Va antes de la ruta con {identificacion}.
personas.MapGet("/mujeres", (IPersonaService servicio) =>
    Results.Ok(servicio.ListarMujeres()))
    .WithSummary("Listar mujeres (consulta la vista vw_PersonasFemeninas)");

// SELECT por identificación
personas.MapGet("/{identificacion}", (string identificacion, IPersonaService servicio) =>
{
    var persona = servicio.BuscarPorIdentificacion(identificacion);
    return persona is null ? Results.NotFound() : Results.Ok(persona);
})
    .WithSummary("Buscar persona por identificación");

// UPDATE solo de la edad, filtrado por identificación
personas.MapPatch("/{identificacion}/edad", (string identificacion, EdadDto dto, IPersonaService servicio) =>
    Ejecutar(() => servicio.ActualizarEdad(identificacion, dto.Edad)
        ? Results.NoContent()
        : Results.NotFound()))
    .WithSummary("Actualizar la edad por identificación");

// DELETE por identificación
personas.MapDelete("/{identificacion}", (string identificacion, IPersonaService servicio) =>
    servicio.Eliminar(identificacion)
        ? Results.NoContent()
        : Results.NotFound())
    .WithSummary("Eliminar persona por identificación");

app.Run();

// ---------- Auxiliares ----------

static IResult Ejecutar(Func<IResult> accion)
{
    try
    {
        return accion();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // restricción UNIQUE/CHECK
    {
        return Results.Conflict(new { error = "La identificación ya existe o un valor no cumple las reglas." });
    }
}
