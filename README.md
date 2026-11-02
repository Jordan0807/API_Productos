# API_Productos

API REST de productos desarrollada con **ASP.NET Core (.NET 8)**, **Entity Framework Core (Code First)** y **SQLite**, con integración de **GraphQL** (HotChocolate) y documentación con **Swagger**. El proyecto aplica principios **SOLID**, **Clean Code** e **Inyección de Dependencias**.

## Integrantes

- Jordan Benjamin
- Romero Lautaro
- Vera Ignacio
- Pouyte Luca
- Dominguez Facundo

---

## 1. Tecnologías

| Tecnología | Uso |
|---|---|
| .NET 8 / ASP.NET Core Web API | Framework de la API REST |
| Entity Framework Core 8 (Code First) | Acceso a datos y migraciones |
| SQLite | Base de datos (archivo `productos.db`) |
| Swashbuckle (Swagger) | Documentación y prueba de los endpoints REST |
| HotChocolate | Consultas y mutaciones GraphQL |
| xUnit | Pruebas automatizadas |

## 2. Modelo de datos

**Producto**

| Campo | Tipo | Regla |
|---|---|---|
| `Id` | int | Clave primaria, autoincremental |
| `Nombre` | string | Obligatorio |
| `Descripcion` | string | Opcional |
| `Precio` | decimal | Debe ser mayor a cero |
| `Stock` | int | No puede ser negativo |

---

## 3. Estructura del proyecto

```
API_Productos/
├── Controllers/
│   └── ProductosController.cs     # Endpoints REST (solo manejo HTTP)
├── DTOs/                          # Objetos de entrada/salida de la API
├── Data/
│   └── AppDbContext.cs            # Contexto de EF Core
├── GraphQL/
│   ├── Query.cs                   # Consultas GraphQL
│   └── Mutation.cs                # Mutaciones GraphQL
├── Interfaces/
│   └── IProductoService.cs        # Contrato de la lógica de negocio
├── Migrations/                    # Migraciones de EF Core
├── Models/
│   └── Producto.cs                # Entidad
├── Services/
│   └── ProductoService.cs         # Lógica de negocio
├── Program.cs                     # Configuración y registro de servicios (DI)
└── API_Productos.Tests/           # Proyecto de pruebas (xUnit)
```

**Flujo de una petición**

```
Cliente → Controller (REST)  ─┐
                              ├→ IProductoService → ProductoService → AppDbContext → SQLite
Cliente → Query/Mutation     ─┘
         (GraphQL)
```

REST y GraphQL comparten el **mismo servicio**, por lo que las reglas de negocio están en un solo lugar.

---

## 4. Instalación y ejecución

### Requisitos previos

- Git
- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 o superior

### Pasos

1. Clonar el repositorio:

```bash
git clone https://github.com/Jordan0807/API_Productos.git
cd API_Productos
```

2. Ejecutar la aplicación:

```bash
dotnet run --launch-profile https
```

La base de datos (`productos.db`) se **crea sola** al iniciar la aplicación, aplicando las migraciones de EF Core. No hace falta ningún paso extra. Dejá la terminal abierta mientras uses la API: si la cerrás, la aplicación se detiene.

### Configuración

La cadena de conexión está en `appsettings.json`:

```json
"ConnectionStrings": {
  "CadenaSQL": "Data Source=productos.db"
}
```

Para usar otra ubicación o archivo, modificar ese valor.

### URLs (entorno Development)

| Recurso | URL |
|---|---|
| Swagger UI | https://localhost:7042/swagger |
| GraphQL (IDE Banana Cake Pepper) | https://localhost:7042/graphql |

Si el navegador muestra una advertencia por el certificado HTTPS de desarrollo, ejecutar una vez `dotnet dev-certs https --trust` y reiniciar la aplicación. También se puede usar `http://localhost:5136` con el perfil `http`.

---

## 5. API REST

Documentada y probada desde **Swagger UI** (`/swagger`).

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/productos` | Lista todos los productos | 200 |
| GET | `/api/productos/{id}` | Obtiene un producto por ID | 200, 404 |
| POST | `/api/productos` | Crea un producto | 201, 400 |
| PUT | `/api/productos/{id}` | Edita un producto existente | 204, 400, 404 |
| DELETE | `/api/productos/{id}` | Elimina un producto | 204, 404 |

**Ejemplo de cuerpo para POST / PUT**

```json
{
  "nombre": "Teclado mecánico",
  "descripcion": "Switches rojos, layout español",
  "precio": 45000.50,
  "stock": 12
}
```

Si los datos no cumplen las reglas (por ejemplo, precio negativo o nombre vacío), la API responde **400 Bad Request** con el detalle del error.

---

## 6. GraphQL

Endpoint: `/graphql`

Se abre el editor Banana Cake Pepper: apretar **Crear documento**, pegar una operación y apretar **Run**. Conviene probar una operación por pestaña.

**Consultas**

```graphql
query {
  productos {
    id
    nombre
    precio
    stock
  }
}
```

```graphql
query {
  productoPorId(id: 1) {
    id
    nombre
    descripcion
  }
}
```

Si el id no existe, `productoPorId` devuelve `null`.

**Mutaciones**

```graphql
mutation {
  crearProducto(input: { nombre: "Mouse", descripcion: "Inalámbrico", precio: 15000, stock: 30 }) {
    id
    nombre
  }
}
```

```graphql
mutation {
  actualizarProducto(id: 1, input: { nombre: "Mouse", descripcion: "Gamer", precio: 18000, stock: 25 })
}
```

```graphql
mutation {
  eliminarProducto(id: 1)
}
```

`actualizarProducto` y `eliminarProducto` devuelven `true` si el producto existía y `false` si no.

---

## 7. Pruebas

Las pruebas verifican las reglas principales del sistema sobre `ProductoService`, usando una base de datos en memoria (EF Core InMemory), por lo que no tocan `productos.db`:

- Crear un producto y obtenerlo por ID.
- Rechazar un precio negativo.
- Rechazar un stock negativo.
- Rechazar un nombre vacío.
- Obtener, editar o eliminar un producto inexistente devuelve "no encontrado".
- Eliminar un producto lo quita del listado.

Ejecutar todas las pruebas:

```bash
dotnet test
```

Resultado esperado: **8 pruebas correctas y 0 con errores**.

---

## 8. Cómo se aplicaron SOLID, Clean Code e Inyección de Dependencias

### Principios SOLID

**S – Responsabilidad Única.** Cada clase tiene un solo motivo para cambiar:
`ProductosController` solo traduce HTTP (códigos de estado, rutas); `ProductoService` contiene las reglas de negocio; `AppDbContext` se encarga de la persistencia; los DTOs definen el contrato de la API; `Query` y `Mutation` solo exponen el servicio vía GraphQL.

**O – Abierto/Cerrado.** Se puede agregar nueva funcionalidad (por ejemplo, un `ICategoriaService`, o nuevos campos en consultas GraphQL) sin modificar las clases existentes.

**L – Sustitución de Liskov.** Cualquier implementación de `IProductoService` puede reemplazar a `ProductoService` sin que el controller ni GraphQL lo noten. Las pruebas aprovechan esto.

**I – Segregación de Interfaces.** `IProductoService` define solo las operaciones de productos que los consumidores necesitan, sin métodos innecesarios.

**D – Inversión de Dependencias.** El controller, `Query` y `Mutation` dependen de la **abstracción** `IProductoService`, no de la clase concreta ni del `DbContext`. Las capas de entrada no conocen cómo se almacenan los datos.

### Inyección de Dependencias

Los servicios se registran en el contenedor de .NET en `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddScoped<IProductoService, ProductoService>();
```

Se usa el ciclo de vida **Scoped** porque `AppDbContext` también es scoped: se crea una instancia por petición HTTP y se comparte dentro de ella. Las dependencias se reciben por **constructor**, nunca con `new`.

### Clean Code

- Nombres descriptivos y consistentes (`ProductoService`, `CrearProductoDto`).
- Métodos cortos con una sola tarea.
- Sin duplicación: REST y GraphQL reutilizan el mismo servicio.
- Validaciones explícitas: los datos inválidos se rechazan con un error claro, en lugar de corregirse en silencio.
- DTOs para no exponer la entidad de base de datos ni permitir que el cliente defina el `Id`.
- Código asíncrono (`async/await`) en todo el acceso a datos.

---

## 9. Demostración sugerida

1. Ejecutar la aplicación y abrir `/swagger`.
2. Crear un producto con `POST` y comprobar el código **201**.
3. Listarlo con `GET /api/productos` y obtenerlo con `GET /api/productos/{id}`.
4. Intentar crear un producto con precio negativo y mostrar el **400**.
5. Editarlo con `PUT` y eliminarlo con `DELETE`.
6. Abrir `/graphql` y ejecutar una consulta y una mutación sobre los mismos datos.
7. Ejecutar `dotnet test` y mostrar las pruebas en verde.