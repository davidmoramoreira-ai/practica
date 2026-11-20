namespace practica.Web;

using practica.Entidades;
using practica.Negocio;

public static class PlatoEndpoints
{
    public static void MapPlatoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/platos");

        grupo.MapGet("/", async (PlatoServicio s) =>
            Results.Ok(await s.ObtenerTodosAsync())).RequireAuthorization("SoloAdmin");

        grupo.MapGet("/{id:int}", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id)))).RequireAuthorization("SoloAdmin");

        grupo.MapPost("/", (Plato p, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                p.Id = await s.CrearAsync(p);
                p.Disponible = true;
                return Results.Created($"/platos/{p.Id}", p);
            })).RequireAuthorization("SoloAdmin");

        grupo.MapPut("/{id:int}", (int id, Plato p, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                p.Id = id;
                await s.ActualizarAsync(p);
                return Results.Ok(p);
            })).RequireAuthorization("SoloAdmin");

        grupo.MapDelete("/{id:int}", (int id, PlatoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                await s.EliminarAsync(id);
                return Results.NoContent(); // 204
            })).RequireAuthorization("SoloAdmin");
    }
}
