using practica.Datos;
using practica.Negocio;

namespace practica.Extensions;

public static class ServiciosRestaurante
{
    public static IServiceCollection AddServiciosRestaurante(this IServiceCollection services)
    {
        services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();
        services.AddScoped<PlatoServicio>();
        return services;
    }
}
