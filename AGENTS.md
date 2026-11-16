# Guía y Reglas del Proyecto (AGENTS.md)

Este repositorio contiene una API REST desarrollada en **.NET 10** (C#) para la gestión de un restaurante (platos, categorías y productos).

---

## 🏗 Arquitectura y Estructura

El proyecto sigue una arquitectura limpia organizada en capas:

- **`Entidades/`**: Modelos de datos del dominio (`Plato`, `Categorias`, `Producto`).
- **`Datos/`**: Acceso a datos mediante ADO.NET (`Microsoft.Data.SqlClient`) y colecciones en memoria (`PlatoRepositorio`, `CategoriaRepositorio`, `ProductoRepositorio`).
- **`Negocio/`**: Reglas de negocio, validaciones y control de excepciones (`PlatoServicio`, `CategoriaServicio`, `ProductoServicio`).
- **`Web/`**: Minimal APIs, configuración de OpenAPI y traducción centralizada de errores HTTP (`PlatoEndpoints`, `CategoriaEndpoints`, `ProductoEndpoints`, `Respuestas`).
- **`Extensions/`**: Métodos de extensión para inyección de dependencias (`ServiciosRestaurante`).

---

## 🗄️ Base de Datos y Configuración

- **Servidor**: SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- **Base de datos**: `RestauranteITQ`
- **Variables de entorno**: Administradas con `DotNetEnv` mediante el archivo `.env` (`ConnectionStrings__Restaurante`).
- **Fallback de configuración**: `appsettings.json` contiene la cadena de conexión por defecto si el `.env` no está presente.

---

## 📋 Convenciones de Endpoints y Respuestas HTTP

El helper `Respuestas.Manejar` traduce las excepciones de negocio a códigos de estado HTTP estándar:

| Excepción | Código HTTP | Significado |
| :--- | :--- | :--- |
| `ArgumentException` | `400 Bad Request` | Parámetros o validaciones inválidas (nombre vacío, precios negativos, longitud de texto excedida) |
| `KeyNotFoundException` | `404 Not Found` | Recurso no encontrado (por ID o borrado lógico) |
| `InvalidOperationException`| `409 Conflict` | Conflicto con reglas de negocio (ej. borrar categoría con platos, nombre de categoría duplicado) |

### Reglas específicas:
- **Borrado lógico en Platos**: La tabla `Platos` utiliza la columna `Disponible (bit)`. Las consultas activas filtran por `WHERE Disponible = 1`. Eliminar un plato actualiza `Disponible = 0`. Consultar un plato con `Disponible = 0` devuelve `404 Not Found`.
- **Integridad referencial**: No se puede eliminar una categoría si tiene platos asociados (devuelve `409 Conflict`).
- **Nombres únicos**: No se permite crear ni actualizar categorías con nombres duplicados (devuelve `409 Conflict`).
- **Validaciones de longitud**: Nombres de platos hasta 100 caracteres, descripciones hasta 300 caracteres, categorías hasta 50 caracteres.

---

## 🚀 Comandos Útiles

```powershell
# Compilar proyecto
dotnet build

# Ejecutar API
dotnet run --project practica/practica.csproj

# Pruebas HTTP
# Usar el archivo practica/practica.http con la extensión REST Client
```
