using practica.Datos;
using practica.Extensions;
using practica.Negocio;
using practica.Web;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar las capas (una línea por clase)
builder.Services.AddScoped<ProductoRepositorio>();
builder.Services.AddScoped<ProductoServicio>();
builder.Services.AddScoped<PlatoRepositorio>();
builder.Services.AddScoped<PlatoServicio>();
builder.Services.AddScoped<CategoriaRepositorio>();
builder.Services.AddScoped<CategoriaServicio>();

var app = builder.Build();

// 2. Productos (en memoria, clase 3)
app.MapGet("/productos", (ProductoServicio servicio) =>
    Results.Ok(servicio.ListaDisponible()));

// 3. Cada módulo registra sus propias rutas
app.MapPlatoEndpoints();
app.MapCategoriaEndpoints();

app.Run();