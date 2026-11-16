// Datos/IUsuarioRepositorio.cs
using practica.Entidades;

namespace practica.Datos;

// El contrato: QUÉ sabe hacer el repositorio de usuarios
public interface IUsuarioRepositorio
{
    Task<List<Usuario>> ListarAsync();
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<bool> ExisteCorreoAsync(string correo);
    Task AgregarAsync(Usuario u);
}