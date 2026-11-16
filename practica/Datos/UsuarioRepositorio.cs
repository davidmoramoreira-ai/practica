using Microsoft.EntityFrameworkCore;
using practica.Entidades;

namespace practica.Datos;

public class UsuarioRepositorio(RestauranteContext db)
: IUsuarioRepositorio
{
    public Task<List<Usuario>> ListarAsync() =>
        db.Usuarios.AsNoTracking()
          .OrderBy(u => u.Nombre).ToListAsync();

    public Task<Usuario?> ObtenerPorCorreoAsync(string correo) =>
        db.Usuarios.FirstOrDefaultAsync(
            u => u.Correo == correo && u.Activo);

    public Task<bool> ExisteCorreoAsync(string correo) =>
        db.Usuarios.AnyAsync(u => u.Correo == correo);

    public async Task AgregarAsync(Usuario u)
    {
        db.Usuarios.Add(u);
        await db.SaveChangesAsync();
    }
}

