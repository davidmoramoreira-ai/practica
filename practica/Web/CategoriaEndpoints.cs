    namespace practica.Web;
using practica.Negocio;
using practica.Entidades;

public static class CategoriaEndpoints
{
    public static void MapCategoriaEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/categorias");   // todas empiezan con /categorias

        grupo.MapGet("/", async (CategoriaServicio s) =>
            Results.Ok(await s.ObtenerTodasAsync()));

        grupo.MapGet("/{id:int}", (int id, CategoriaServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id))));

        // La relación: todos los platos de una categoría
        grupo.MapGet("/{id:int}/platos", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorCategoriaAsync(id))));

        grupo.MapPost("/", (Categorias c, CategoriaServicio s) =>
         Respuestas.Manejar(async () =>
         {
             c.Id = await s.AgregarAsync(c);
             return Results.Created($"/categorias/{c.Id}", c);
         }));

        grupo.MapPut("/{id:int}", (int id, Categorias c, CategoriaServicio s) =>
            Respuestas.Manejar(async () =>
            {
                c.Id = id;
                await s.ActualizarAsync(c);
                return Results.Ok(c);
            }));

        grupo.MapDelete("/{id:int}", (int id, CategoriaServicio s) =>
            Respuestas.Manejar(async () =>
            {
                await s.EliminarAsync(id);
                return Results.NoContent();
            }));
    }
}