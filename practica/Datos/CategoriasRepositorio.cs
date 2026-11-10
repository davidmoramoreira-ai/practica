namespace practica.Datos;

using practica.Entidades;
using Microsoft.Data.SqlClient;

public class CategoriaRepositorio(IConfiguration config)
{
    private readonly string _cn = config.GetConnectionString("Restaurante")!;

    public async Task<List<Categorias>> ObtenerTodasAsync()
    {
        var lista = new List<Categorias>();
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, Nombre FROM Categorias ORDER BY Nombre", cn);
        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
            lista.Add(new Categorias { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
        return lista;
    }

    public async Task<Categorias?> ObtenerPorIdAsync(int id)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, Nombre FROM Categorias WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var dr = await cmd.ExecuteReaderAsync();
        if (!await dr.ReadAsync()) return null;
        return new Categorias { Id = dr.GetInt32(0), Nombre = dr.GetString(1) };
    }

    public async Task<int> AgregarAsync(Categorias c)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "INSERT INTO Categorias (Nombre) OUTPUT INSERTED.Id VALUES (@nombre)", cn);
        cmd.Parameters.AddWithValue("@nombre", c.Nombre);
        return (int)(await cmd.ExecuteScalarAsync())!;   // el Id que generó SQL Server
    }

    public async Task<int> ActualizarAsync(Categorias c)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "UPDATE Categorias SET Nombre = @nombre WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@nombre", c.Nombre);
        cmd.Parameters.AddWithValue("@id", c.Id);
        return await cmd.ExecuteNonQueryAsync();   // filas afectadas
    }

    public async Task<int> EliminarAsync(int id)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "DELETE FROM Categorias WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync();
    }

    // ¿Cuántos platos usan esta categoría?
    public async Task<int> ContarPlatosAsync(int id)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT COUNT(*) FROM Platos WHERE CategoriaId = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

   
}