using practica.Endpoints;
using practica.Extensions;

// Carga el archivo .env antes de crear el builder
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiciosRestaurante();

var app = builder.Build();

app.MapPlatoEndpoints();

app.Run();