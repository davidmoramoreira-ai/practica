namespace practica.Entidades;
// Lo que ENTRA al iniciar sesión
public record LoginRequest(string Correo, string Password);

// Lo que SALE: el token y los datos públicos del usuario
public record LoginRespuesta(
    string Token,
    DateTime Expira,
    UsuarioDto Usuario);