namespace PruebaPersona.Models;

public class Persona
{
    public string Identificacion { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public int Edad { get; set; }

    // Valores permitidos: "Masculino" o "Femenino"
    public string Genero { get; set; } = string.Empty;
}
