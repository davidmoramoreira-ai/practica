using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace practica.Web
{
    // Un solo lugar que traduce las excepciones del negocio a códigos HTTP
    public static class Respuestas
    {
        public static async Task<IResult> Manejar(Func<Task<IResult>> accion)
        {
            try
            {
                return await accion();
            }
            catch (ArgumentException ex)          // datos inválidos
            {
                return Results.BadRequest(new { error = ex.Message });          // 400
            }
            catch (KeyNotFoundException ex)       // no existe
            {
                return Results.NotFound(new { error = ex.Message });            // 404
            }
            catch (InvalidOperationException ex)  // choca con una regla
            {
                return Results.Conflict(new { error = ex.Message });            // 409
            }
        }
    }
}