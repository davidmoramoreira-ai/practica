namespace practica.Negocio;

using practica.Datos;
using practica.Entidades;

public class ProductoServicio(IProductoRepositorio repo)
{
    public List<Producto> ObtenerTodos() => repo.ObtenerTodos();

    public List<Producto> ListaDisponible() =>
        repo.ObtenerTodos().Where(p => p.Stock > 0).ToList();

    public Producto Buscar(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id debe ser mayor que cero.");

        var prod = repo.ObtenerPorId(id);
        if (prod is null)
            throw new KeyNotFoundException($"No existe el producto {id}.");

        return prod;
    }
}
