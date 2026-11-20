namespace practica.Negocio;

using Microsoft.Data.SqlClient;
using practica.Datos;
using practica.Entidades;

public class PlatoServicio(IPlatoRepositorio repo, ICategoriaRepositorio categorias)
{
    public Task<List<Plato>> ObtenerTodosAsync() => repo.ObtenerTodosAsync();

    public async Task<Plato> ObtenerPorIdAsync(int id)
    {
        var plato = await repo.ObtenerPorIdAsync(id);
        if (plato is null)
            throw new KeyNotFoundException($"No existe el plato {id}.");
        return plato;
    }

    // Método para obtener los platos de una categoría específica
    public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        var categoria = await categorias.ObtenerPorIdAsync(categoriaId);
        if (categoria is null)
            throw new KeyNotFoundException($"No existe la categoría {categoriaId}.");

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

    public async Task ActualizarAsync(Plato p)
    {
        if (p.Id <= 0)
            throw new ArgumentException("Debe indicar un plato válido.");
        Validar(p);
        p.Nombre = p.Nombre.Trim();
        try
        {
            var filas = await repo.ActualizarAsync(p);
            if (filas == 0)
                throw new KeyNotFoundException($"No existe el plato {p.Id}.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ArgumentException($"La categoría con ID {p.CategoriaId} no existe.");
        }
    }

    public async Task EliminarAsync(int id)
    {
        var filas = await repo.EliminarAsync(id);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe el plato {id}.");
    }

    private static void Validar(Plato p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre))
            throw new ArgumentException("El nombre del plato es obligatorio.");
        if (p.Nombre.Length > 100)
            throw new ArgumentException("El nombre del plato no puede superar los 100 caracteres.");
        if (p.Descripcion?.Length > 300)
            throw new ArgumentException("La descripción del plato no puede superar los 300 caracteres.");
        if (p.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0.");
        if (p.CategoriaId <= 0)
            throw new ArgumentException("Debe indicar una categoría válida.");
    }
}
