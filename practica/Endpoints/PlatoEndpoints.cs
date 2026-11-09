using practica.Entidades;
using practica.Negocio;

namespace practica.Endpoints;

public static class PlatoEndpoints
{
    public static void MapPlatoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/platos");

        grupo.MapGet("/", async (PlatoServicio s) =>
            Results.Ok(await s.ListarAsync()));

        grupo.MapGet("/{id:int}", async (int id, PlatoServicio s) =>
        {
            var plato = await s.ObtenerPorIdAsync(id);
            return plato is null ? Results.NotFound() : Results.Ok(plato);
        });

        grupo.MapPost("/", async (Plato p, PlatoServicio s) =>
        {
            try
            {
                p.Id = await s.CrearAsync(p);
                return Results.Created($"/platos/{p.Id}", p);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        grupo.MapPut("/{id:int}", async (int id, Plato p, PlatoServicio s) =>
        {
            try
            {
                p.Id = id;
                return await s.ActualizarAsync(p) ? Results.Ok(p) : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        grupo.MapDelete("/{id:int}", async (int id, PlatoServicio s) =>
            await s.EliminarAsync(id) ? Results.NoContent() : Results.NotFound());
    }
}
