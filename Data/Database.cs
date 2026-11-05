using Microsoft.Data.Sqlite;

namespace PruebaPersona.Data;

public static class Database
{
    // El archivo persona.db se crea en la carpeta desde la que se ejecuta la app.
    public const string ConnectionString = "Data Source=persona.db";

    public static SqliteConnection Abrir()
    {
        var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        return conn;
    }

    /// <summary>Crea la tabla Persona y la vista vw_PersonasFemeninas si no existen.</summary>
    public static void Inicializar()
    {
        using var conn = Abrir();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Persona (
                Identificacion TEXT    NOT NULL PRIMARY KEY,
                Nombres        TEXT    NOT NULL,
                Apellidos      TEXT    NOT NULL,
                Edad           INTEGER NOT NULL CHECK (Edad BETWEEN 0 AND 150),
                Genero         TEXT    NOT NULL CHECK (Genero IN ('Masculino', 'Femenino'))
            );

            CREATE VIEW IF NOT EXISTS vw_PersonasFemeninas AS
            SELECT Identificacion, Nombres, Apellidos, Edad, Genero
            FROM Persona
            WHERE Genero = 'Femenino';
            """;
        cmd.ExecuteNonQuery();
    }
}
