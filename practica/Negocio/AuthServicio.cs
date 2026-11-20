using practica.Datos;
using practica.Entidades;

namespace practica.Negocio;

public class AuthServicio(
IUsuarioRepositorio repo, TokenServicio tokens)
{
    public async Task<LoginRespuesta?> LoginAsync(LoginRequest r)
    {
        // 1. El mismo correo normalizado que en el registro
        var correo = (r.Correo ?? "").Trim().ToLower();
        var u = await repo.ObtenerPorCorreoAsync(correo);

        // 2. ¿Existe y la contraseña coincide con su hash?
        if (u is null ||
            !BCrypt.Net.BCrypt.Verify(r.Password ?? "", u.PasswordHash))
            return null;   // → 401, sin decir cuál de los dos falló

        // 3. Todo bien: fabricar el token
        var (token, expira) = tokens.Crear(u);
        return new LoginRespuesta(token, expira,
            new UsuarioDto(u.Id, u.Nombre, u.Correo, u.Rol));
    }
}