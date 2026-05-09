# StockFlow API

Backend REST API para el sistema de gestión de inventario StockFlow, desarrollado con ASP.NET Core y C#.

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Neon)
- JWT Authentication
- BCrypt para hash de contraseñas

## Requisitos

- .NET 10 SDK
- PostgreSQL o cuenta en [Neon](https://neon.tech)

## Instalación

```bash
git clone https://github.com/tu-usuario/stockflow-api
cd stockflow-api
dotnet restore
```

Configurá las variables en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu-connection-string"
  },
  "Jwt": {
    "Key": "tu-secret-key",
    "Issuer": "StockFlow.API",
    "Audience": "StockFlow.Client"
  }
}
```

Aplicá las migraciones:

```bash
dotnet ef database update
```

Corré el proyecto:

```bash
dotnet run
```

La API estará disponible en `http://localhost:5050`. La documentación OpenAPI en `http://localhost:5050/openapi/v1.json`.

## Endpoints principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | /api/auth/register | Registrar usuario |
| POST | /api/auth/login | Iniciar sesión |
| GET | /api/products | Listar productos |
| GET | /api/products/low-stock | Productos con stock bajo |
| POST | /api/products | Crear producto |
| PUT | /api/products/{id} | Actualizar producto |
| DELETE | /api/products/{id} | Eliminar producto |
| GET | /api/categories | Listar categorías |
| GET | /api/suppliers | Listar proveedores |
| GET | /api/stockmovements | Listar movimientos |
| POST | /api/stockmovements | Registrar movimiento (actualiza stock automáticamente) |

## Autenticación

Todos los endpoints excepto `/api/auth/*` requieren un token JWT en el header:

```
Authorization: Bearer {token}
```

## Roles

- **Admin** → acceso completo (crear, editar, eliminar)
- **Employee** → solo lectura y registro de movimientos

## Estructura del proyecto

```
StockFlow.API/
├── Controllers/    # Endpoints de la API
├── Data/           # DbContext
├── DTOs/           # Objetos de transferencia de datos
├── Models/         # Entidades de la base de datos
└── Services/       # TokenService (JWT)
```
