using practica.Entidades;
using practica.Datos;
namespace practica.Negocio
{
    public class ProductoServicio
    {

        private readonly ProductoRepositorio _repo;

        public ProductoServicio(ProductoRepositorio repo)
        {
            _repo = repo;
        }

        public List<Producto> ListaDisponible() =>
            _repo.ObtenerTodos().Where(p => p.Stock > 0).ToList();

        public Producto? Buscar (int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor que cero");
            return _repo.ObtenerPorId(id);
        }

    }
}
