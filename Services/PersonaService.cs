using Microsoft.Data.Sqlite;
using PruebaPersona.Data;
using PruebaPersona.Models;

namespace PruebaPersona.Services;

public class PersonaService : IPersonaService
{
    private const string Columnas = "Identificacion, Nombres, Apellidos, Edad, Genero";

    // CREATE: insertar un registro
    public void Insertar(Persona persona)
    {
        Validar(persona);

        using var conn = Database.Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO Persona (Identificacion, Nombres, Apellidos, Edad, Genero)
            VALUES ($id, $nombres, $apellidos, $edad, $genero);
            """;
        cmd.Parameters.AddWithValue("$id", persona.Identificacion);
        cmd.Parameters.AddWithValue("$nombres", persona.Nombres);
        cmd.Parameters.AddWithValue("$apellidos", persona.Apellidos);
        cmd.Parameters.AddWithValue("$edad", persona.Edad);
        cmd.Parameters.AddWithValue("$genero", persona.Genero);
        cmd.ExecuteNonQuery();
    }

    // READ: buscar por identificación
    public Persona? BuscarPorIdentificacion(string identificacion)
    {
        using var conn = Database.Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {Columnas} FROM Persona WHERE Identificacion = $id;";
        cmd.Parameters.AddWithValue("$id", identificacion);

        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Mapear(reader) : null;
    }

    // READ: listar mujeres consultando la vista
    public List<Persona> ListarMujeres()
    {
        var lista = new List<Persona>();

        using var conn = Database.Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {Columnas} FROM vw_PersonasFemeninas ORDER BY Apellidos, Nombres;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Mapear(reader));
        }
        return lista;
    }

    // UPDATE: únicamente la edad, filtrando por identificación
    public bool ActualizarEdad(string identificacion, int edad)
    {
        ValidarEdad(edad);

        using var conn = Database.Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Persona SET Edad = $edad WHERE Identificacion = $id;";
        cmd.Parameters.AddWithValue("$edad", edad);
        cmd.Parameters.AddWithValue("$id", identificacion);

        return cmd.ExecuteNonQuery() > 0;
    }

    // DELETE: por identificación
    public bool Eliminar(string identificacion)
    {
        using var conn = Database.Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Persona WHERE Identificacion = $id;";
        cmd.Parameters.AddWithValue("$id", identificacion);

        return cmd.ExecuteNonQuery() > 0;
    }

    // ---------- Auxiliares ----------

    private static Persona Mapear(SqliteDataReader r) => new()
    {
        Identificacion = r.GetString(0),
        Nombres = r.GetString(1),
        Apellidos = r.GetString(2),
        Edad = r.GetInt32(3),
        Genero = r.GetString(4)
    };

    private static void Validar(Persona p)
    {
        if (string.IsNullOrWhiteSpace(p.Identificacion))
            throw new ArgumentException("La identificación es obligatoria.");
        if (string.IsNullOrWhiteSpace(p.Nombres) || string.IsNullOrWhiteSpace(p.Apellidos))
            throw new ArgumentException("Nombres y apellidos son obligatorios.");
        if (p.Genero != "Masculino" && p.Genero != "Femenino")
            throw new ArgumentException("El género debe ser Masculino o Femenino.");
        ValidarEdad(p.Edad);
    }

    private static void ValidarEdad(int edad)
    {
        if (edad < 0 || edad > 150)
            throw new ArgumentOutOfRangeException(nameof(edad), "La edad debe estar entre 0 y 150.");
    }
}
