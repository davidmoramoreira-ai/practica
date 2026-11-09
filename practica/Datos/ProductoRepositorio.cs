using practica.Entidades;
namespace practica.Datos

{
    public class ProductoRepositorio
    {

        private static readonly List<Producto> _datos = new()
        {
          new Producto { Id = 1, Nombre = "Mouse", Precio = 12.50m, Stock = 10},
          new Producto { Id = 2, Nombre = "Teclado", Precio = 25m, Stock = 0},
          new Producto { Id = 3, Nombre = "Monitor", Precio = 180m, Stock = 5},
        };

        public List<Producto> ObtenerTodos() => _datos;

        public Producto? ObtenerPorId(int id) => 
            _datos.FirstOrDefault(p => p.Id == id);

    }
}
