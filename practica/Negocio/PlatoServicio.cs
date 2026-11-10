using Microsoft.Data.SqlClient;
using practica.Entidades;
using practica.Datos;
namespace practica.Negocio;

public class PlatoServicio(PlatoRepositorio repo, CategoriaRepositorio categorias)
{
    public Task<List<Plato>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

    public Task<Plato?> ObtenerPorIdAsync(int id) => repo.ObtenerPorIdAsync(id);

    // NUEVO: Método para obtener los platos de una categoría específica
    public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        var lista = await repo.ObtenerTodosAsync();
        return lista.Where(p => p.CategoriaId == categoriaId).ToList();
    }

    public async Task<int> CrearAsync(Plato p)
    {
        Validar(p);
        p.Nombre = p.Nombre.Trim();
        try
        {
            return await repo.AgregarAsync(p);
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ArgumentException($"La categoría {p.CategoriaId} no existe.");
        }
    }

    public async Task<bool> ActualizarAsync(Plato p)
    {
        if (p.Id <= 0)
            throw new ArgumentException("Debe indicar un plato válido.");
        Validar(p);
        p.Nombre = p.Nombre.Trim();
        try
        {
            return await repo.ActualizarAsync(p) > 0;
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ArgumentException($"La categoría con ID {p.CategoriaId} no existe.");
        }
    }

    public async Task<bool> EliminarAsync(int id) => await repo.EliminarAsync(id) > 0;

    private static void Validar(Plato p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new ArgumentException("El nombre del plato es obligatorio.");
        if (p.Nombre.Length > 100)
            throw new ArgumentException("El nombre del plato no puede superar los 100 caracteres.");
        if (p.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0.");
        if (p.CategoriaId <= 0)
            throw new ArgumentException("Debe indicar una categoría válida.");
    }
}