using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using practica.Entidades;
using System.Text;

namespace practica.Negocio;

// Fabrica el JWT de un usuario que ya inició sesión
public class TokenServicio(IConfiguration config)
{
    public (string Token, DateTime Expira) Crear(Usuario u)
    {
        var jwt = config.GetSection("Jwt");

        // ¿Hasta cuándo sirve? (480 min = un turno)
        var expira = DateTime.UtcNow.AddMinutes(
            jwt.GetValue<int>("MinutosExpiracion"));

        // La clave secreta, convertida en bytes
        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!));
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = jwt["Issuer"],
            Audience = jwt["Audience"],
            Expires = expira,

            // PAYLOAD: lo que el token dice del usuario
            Claims = new Dictionary<string, object>
            {
                ["sub"] = u.Id.ToString(),
                ["name"] = u.Nombre,
                ["email"] = u.Correo,
                ["role"] = u.Rol
            },

            // SIGNATURE: HMAC-SHA256 con la clave secreta
            SigningCredentials = new SigningCredentials(
             clave, SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return (token, expira);
    }   // fin de Crear
}   // fin de TokenServicio