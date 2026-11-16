namespace practica.Datos;
using Microsoft.EntityFrameworkCore;
using practica.Entidades;

public class RestauranteContext(
    DbContextOptions<RestauranteContext> options)
    : DbContext(options)
{
    public DbSet<Plato> Platos => Set<Plato>();
    public DbSet<Categorias> Categorias => Set<Categorias>();
    public DbSet<Mesa> Mesas => Set<Mesa>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Plato>(e =>
            {
                e.Property(p => p.Precio).HasPrecision(10, 2);
                e.Property(p => p.Descripcion).IsRequired(false);
                e.Ignore(p => p.CategoriaId);   // viene del JOIN
                e.HasQueryFilter(p => p.Disponible);
            });
        }
    }
