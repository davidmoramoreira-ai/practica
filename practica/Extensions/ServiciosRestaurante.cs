using practica.Datos;
using practica.Negocio;

namespace practica.Extensions;

public static class ServiciosRestaurante
{
    public static IServiceCollection AddServiciosRestaurante(this IServiceCollection services)
    {
        // 1. Repositorios
        services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
        services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();
        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

        // 2. Servicios
        services.AddScoped<ProductoServicio>();
        services.AddScoped<PlatoServicio>();
        services.AddScoped<CategoriaServicio>();

        return services;
    }
}
