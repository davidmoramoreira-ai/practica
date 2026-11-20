namespace practica.Datos;

using practica.Entidades;

public interface IPlatoRepositorio
{
    Task<List<Plato>> ObtenerTodosAsync();
    Task<Plato?> ObtenerPorIdAsync(int id);
    Task<int> AgregarAsync(Plato p);
    Task<int> ActualizarAsync(Plato p);
    Task<int> EliminarAsync(int id);
}
