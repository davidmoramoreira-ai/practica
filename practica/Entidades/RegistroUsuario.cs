namespace practica.Entidades;
// Lo que ENTRA al registrar
// Lo que ENTRA al registrar
public record RegistroUsuario(
    string Nombre, string Correo,
    string Password, string Rol);

// Lo que SALE de la API: sin PasswordHash
public record UsuarioDto(
    int Id, string Nombre, string Correo, string Rol);