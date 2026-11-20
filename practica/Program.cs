using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using practica.Datos;
using practica.Entidades;
using practica.Negocio;
using practica.Web;

// Cargar variables de entorno desde .env antes de inicializar la app
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Obtención de la cadena de conexión
var cn = builder.Configuration.GetConnectionString("Restaurante");

// ==========================================
// 1. INYECCIÓN DE DEPENDENCIAS Y SERVICIOS
// ==========================================

builder.Services.AddOpenApi();

// Plato Servicio
builder.Services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();
builder.Services.AddScoped<PlatoServicio>();

// Categoria Servicio
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<CategoriaServicio>();

// Producto Servicio
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ProductoServicio>();

// Mesa Servicio
builder.Services.AddScoped<IMesaRepositorio, MesaRepositorio>();
builder.Services.AddScoped<MesaServicio>();

// Usuario Servicio
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<UsuarioServicio>();

// JWT - Login
builder.Services.AddScoped<TokenServicio>();
builder.Services.AddScoped<AuthServicio>();

// Configuración de Entity Framework Core con SQL Server
builder.Services.AddDbContext<RestauranteContext>(
    o => o.UseSqlServer(cn));

// ==========================================
// 2. CONFIGURACIÓN DE SEGURIDAD (JWT)
// ==========================================
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        // Deja "name" y "role" con el mismo nombre del token
        o.MapInboundClaims = false;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,      // vence a la hora exacta
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)),
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SoloAdmin",
        p => p.RequireRole(Roles.Administrador))

    .AddPolicy("Personal",
        p => p.RequireRole(Roles.Administrador,
                           Roles.Mesero, Roles.Cocina))
    .AddPolicy("Mesero",
        p => p.RequireRole(Roles.Mesero))
    .AddPolicy("Cocina",
        p => p.RequireRole(Roles.Cocina));


var app = builder.Build();

// ==========================================
// 3. CONFIGURACIÓN DEL PIPELINE HTTP
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

// ==========================================
// 4. MAPEO DE ENDPOINTS
// ==========================================
app.MapPlatoEndpoints();
app.MapCategoriaEndpoints();
app.MapProductoEndpoints();
app.MapMesaEndpoints();
app.MapUsuarioEndpoints();
app.MapAuthEndpoints();

app.Run();