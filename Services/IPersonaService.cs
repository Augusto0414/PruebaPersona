using PruebaPersona.Models;

namespace PruebaPersona.Services;

public interface IPersonaService
{
    void Insertar(Persona persona);
    Persona? BuscarPorIdentificacion(string identificacion);
    List<Persona> ListarMujeres();
    bool ActualizarEdad(string identificacion, int edad);
    bool Eliminar(string identificacion);
}
