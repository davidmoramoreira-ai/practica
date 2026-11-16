using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using practica.Datos;
using practica.Extensions;
using practica.Negocio;
using practica.Web;

// Cargar variables de entorno desde .env antes de inicializar la app
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// 1. Inyección de dependencias (capas y OpenAPI)
builder.Services.AddServiciosRestaurante();
builder.Services.AddOpenApi();

var cn = builder.Configuration.GetConnectionString("Restaurante");
builder.Services.AddDbContext<RestauranteContext>(
    o => o.UseSqlServer(cn));

builder.Services.AddScoped<IMesaRepositorio, MesaRepositorio>();
builder.Services.AddScoped<MesaServicio>();

// NUEVO: repositorio y servicio de usuarios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<UsuarioServicio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 2. Registro de rutas de cada módulo
app.MapPlatoEndpoints();
app.MapCategoriaEndpoints();
app.MapProductoEndpoints();
app.MapMesaEndpoints();
app.MapUsuarioEndpoints();
app.Run();