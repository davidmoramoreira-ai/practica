using practica.Entidades;
using practica.Negocio;
using System.Security.Claims;

namespace practica.Web;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/auth");

        // LOGIN: público → 200 con el token, o 401
        g.MapPost("/login", async (LoginRequest r, AuthServicio s) =>
            await s.LoginAsync(r) is { } res
                ? Results.Ok(res)
                : Results.Unauthorized());

        // REGISTRO: solo el administrador crea cuentas
        g.MapPost("/registro", (RegistroUsuario r, UsuarioServicio s) =>
            Respuestas.Manejar(async () =>
            {
                var u = await s.RegistrarAsync(r);
                return Results.Created($"/usuarios/{u.Id}", u);
            }))
         .RequireAuthorization("SoloAdmin");

        // YO: ¿quién soy según mi token?
        g.MapGet("/yo", (ClaimsPrincipal user) => Results.Ok(new
        {
            id = user.FindFirstValue("sub"),
            nombre = user.Identity?.Name,
            rol = user.FindFirstValue("role")
        }))
         .RequireAuthorization();
    }   // fin de MapAuthEndpoints
}   // fin de AuthEndpoints