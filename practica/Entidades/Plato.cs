namespace practica.Entidades
{
    public class Plato
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        public int CategoriaId { get; set; }

        public string Catrgorias { get; set; } = "";
    }
}
