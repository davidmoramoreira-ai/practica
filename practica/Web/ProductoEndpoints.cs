namespace practica.Web;

using practica.Negocio;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/productos");

        // Lista de productos con stock disponible
        grupo.MapGet("/", (ProductoServicio s) =>
            Results.Ok(s.ListaDisponible()));

        // Catálogo completo de productos
        grupo.MapGet("/todos", (ProductoServicio s) =>
            Results.Ok(s.ObtenerTodos()));

        // Buscar producto por Id
        grupo.MapGet("/{id:int}", (int id, ProductoServicio s) =>
            Respuestas.Manejar(() => Results.Ok(s.Buscar(id))));
    }
}
