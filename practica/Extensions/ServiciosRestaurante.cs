using practica.Datos;
using practica.Negocio;

namespace practica.Extensions;

public static class ServiciosRestaurante
{
    public static IServiceCollection AddServiciosRestaurante(this IServiceCollection services)
    {
        // 1. Repositorios
        services.AddScoped<PlatoRepositorio>();
        services.AddScoped<CategoriaRepositorio>(); // <-- Añade esta línea

        // 2. Servicios
        services.AddScoped<PlatoServicio>();

        return services;
    }
}