using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using practica.Entidades;

public class PlatoRepositorio(IConfiguration config)
{
    private readonly string _cn = config.GetConnectionString("Restaurante")!;

    public async Task<List<Plato>> ObtenerTodosAsync()
    {
        var lista = new List<Plato>();
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        var cmd = new SqlCommand(
            "SELECT Id, Nombre, Precio, CategoriaId FROM Platos WHERE Disponible = 1 ORDER BY Nombre", cn);
        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
            lista.Add(new Plato
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Precio = dr.GetDecimal(2),
                CategoriaId = dr.GetInt32(3)
            });
        return lista;
    }

    public async Task<Plato?> ObtenerPorIdAsync(int id)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        var cmd = new SqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Disponible, CategoriaId FROM Platos WHERE Id = @id AND Disponible = 1", cn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var dr = await cmd.ExecuteReaderAsync();
        if (!await dr.ReadAsync()) return null;
        return new Plato
        {
            Id = dr.GetInt32(0),
            Nombre = dr.GetString(1),
            Descripcion = dr.IsDBNull(2) ? "" : dr.GetString(2),
            Precio = dr.GetDecimal(3),
            Disponible = dr.GetBoolean(4),
            CategoriaId = dr.GetInt32(5)
        };
    }

    public async Task<int> AgregarAsync(Plato p)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            @"INSERT INTO Platos (Nombre, Descripcion, Precio, CategoriaId)
              OUTPUT INSERTED.Id
              VALUES (@nombre, @desc, @precio, @cat)", cn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
        return (int)(await cmd.ExecuteScalarAsync())!;
    }

    public async Task<int> ActualizarAsync(Plato p)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "UPDATE Platos SET Nombre = @nombre, Descripcion = @desc, Precio = @precio, Disponible = @disp, CategoriaId = @cat WHERE Id = @id AND Disponible = 1", cn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@disp", p.Disponible);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
        cmd.Parameters.AddWithValue("@id", p.Id);
        return await cmd.ExecuteNonQueryAsync();
    }

    // Borrado lógico: Disponible = 0
    public async Task<int> EliminarAsync(int id)
    {
        await using var cn = new SqlConnection(_cn);
        await cn.OpenAsync();
        await using var cmd = new SqlCommand(
            "UPDATE Platos SET Disponible = 0 WHERE Id = @id AND Disponible = 1", cn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync();
    }
}