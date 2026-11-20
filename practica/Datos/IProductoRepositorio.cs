namespace practica.Datos;

using practica.Entidades;

public interface IProductoRepositorio
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
}
