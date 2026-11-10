namespace practica.Negocio;
using practica.Entidades;
using practica.Datos;

public class CategoriaServicio(CategoriaRepositorio repo)
{
    public Task<List<Categorias>> ObtenerTodasAsync() => repo.ObtenerTodasAsync();

    public async Task<Categorias> ObtenerPorIdAsync(int id)
    {
        var categoria = await repo.ObtenerPorIdAsync(id);
        if (categoria is null)
            throw new KeyNotFoundException($"No existe la categoría {id}.");   // 404
        return categoria;
    }

    public async Task<int> AgregarAsync(Categorias c)
    {
        Validar(c);
        return await repo.AgregarAsync(c);
    }

    public async Task ActualizarAsync(Categorias c)
    {
        Validar(c);
        var filas = await repo.ActualizarAsync(c);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe la categoría {c.Id}.");   // 404
    }

    public async Task EliminarAsync(int id)
    {
        // Regla de negocio: no se borra una categoría que tiene platos
        var platos = await repo.ContarPlatosAsync(id);
        if (platos > 0)
            throw new InvalidOperationException(
                $"La categoría {id} tiene {platos} platos. Muévelos o elimínalos primero.");   // 409

        var filas = await repo.EliminarAsync(id);
        if (filas == 0)
            throw new KeyNotFoundException($"No existe la categoría {id}.");   // 404
    }

    private static void Validar(Categorias c)
    {
        if (string.IsNullOrWhiteSpace(c.Nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");   // 400
        if (c.Nombre.Length > 50)
            throw new ArgumentException("El nombre no puede superar los 50 caracteres.");
        c.Nombre = c.Nombre.Trim();
    }
}

