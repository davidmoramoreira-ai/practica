namespace practica.Datos;

using practica.Entidades;

public interface ICategoriaRepositorio
{
    Task<List<Categorias>> ObtenerTodasAsync();
    Task<Categorias?> ObtenerPorIdAsync(int id);
    Task<int> AgregarAsync(Categorias c);
    Task<int> ActualizarAsync(Categorias c);
    Task<int> EliminarAsync(int id);
    Task<int> ContarPlatosAsync(int id);
}
