namespace practica.Web;

// Un solo lugar que traduce las excepciones del negocio a códigos HTTP
public static class Respuestas
{
    public static async Task<IResult> Manejar(Func<Task<IResult>> accion)
    {
        try
        {
            return await accion();
        }
        catch (ArgumentException ex)          // 400 Bad Request
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)       // 404 Not Found
        {
            return Results.NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)  // 409 Conflict
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }

    public static IResult Manejar(Func<IResult> accion)
    {
        try
        {
            return accion();
        }
        catch (ArgumentException ex)          // 400 Bad Request
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)       // 404 Not Found
        {
            return Results.NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)  // 409 Conflict
        {
            return Results.Conflict(new { error = ex.Message });
        }
    }
}
