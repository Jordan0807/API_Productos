# Guía de uso y verificación

Esta guía explica cómo ejecutar la API de productos y comprobar que todo funciona. Sirve tanto para el grupo como para quien evalúe el trabajo.

## 1. Requisitos

- Git
- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 o superior

## 2. Descargar y ejecutar

```bash
git clone https://github.com/Jordan0807/API_Productos.git
cd API_Productos
dotnet run --launch-profile https
```

Tiene que aparecer `Now listening on: https://localhost:7042`. La base de datos (`productos.db`) se crea sola al iniciar. **Dejá la terminal abierta**: si la cerrás, la API se detiene.

> Si el navegador muestra una advertencia de certificado, ejecutá una vez `dotnet dev-certs https --trust` y reiniciá la API.

## 3. Pruebas automáticas

En otra terminal, desde la misma carpeta:

```bash
dotnet test
```

Resultado esperado: **8 pruebas correctas y 0 con errores**. Estas pruebas usan una base en memoria, no tocan `productos.db`.

## 4. Verificar la API REST (Swagger)

Abrir `https://localhost:7042/swagger`. Para cada prueba: clic en el endpoint, **Try it out**, completar y **Execute**.

| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | `POST /api/Productos` con el cuerpo de abajo | **201** y el producto con su `id` |
| 2 | `GET /api/Productos` | **200** y la lista con el producto creado |
| 3 | `GET /api/Productos/{id}` con el `id` creado | **200** y el producto |
| 4 | `GET /api/Productos/999` | **404** |
| 5 | `PUT /api/Productos/{id}` con otros datos | **204** (sin contenido) |
| 6 | `POST /api/Productos` con `"precio": -5` | **400** con mensaje de validación |
| 7 | `DELETE /api/Productos/{id}` | **204** |
| 8 | `GET /api/Productos/{id}` del producto borrado | **404** |

Cuerpo para el POST y el PUT:

```json
{
  "nombre": "Mouse",
  "descripcion": "Inalámbrico",
  "precio": 15000,
  "stock": 30
}
```

## 5. Verificar GraphQL

Abrir `https://localhost:7042/graphql`. Se abre el editor Banana Cake Pepper: apretar **Crear documento**, pegar una operación y apretar **Run**. Probar **una operación por pestaña**.

**Listar productos:**

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

**Consultar por ID** (si no existe, devuelve `null`):

```graphql
query {
  productoPorId(id: 1) {
    id
    nombre
    descripcion
  }
}
```

**Crear:**

```graphql
mutation {
  crearProducto(input: { nombre: "Teclado", descripcion: "Mecánico", precio: 45000, stock: 10 }) {
    id
    nombre
  }
}
```

**Actualizar** (devuelve `true` si existe, `false` si no):

```graphql
mutation {
  actualizarProducto(id: 1, input: { nombre: "Teclado gamer", descripcion: "RGB", precio: 52000, stock: 8 })
}
```

**Eliminar** (devuelve `true` si existía, `false` si no):

```graphql
mutation {
  eliminarProducto(id: 1)
}
```

**Regla del precio:** esta mutación tiene que devolver un error y **no** crear el producto:

```graphql
mutation {
  crearProducto(input: { nombre: "Prueba", descripcion: "x", precio: -5, stock: 1 }) {
    id
  }
}
```

## 6. Comprobar que REST y GraphQL comparten los datos

1. Crear un producto desde GraphQL con `crearProducto`.
2. Ejecutar `GET /api/Productos` en Swagger: tiene que aparecer ese producto.

Esto demuestra que ambos usan el mismo servicio (`IProductoService`) y la misma base.

## 7. Checklist final

- [ ] `dotnet test` muestra 8 pruebas correctas
- [ ] Swagger carga y los 5 endpoints responden con los códigos esperados
- [ ] El precio negativo devuelve 400 en REST
- [ ] GraphQL lista, crea, actualiza y elimina productos
- [ ] El precio negativo es rechazado en GraphQL
- [ ] Un producto creado en GraphQL aparece en Swagger

## 8. Dónde está cada requisito en el código

| Requisito de la consigna | Dónde verlo |
|---|---|
| Endpoints REST (CRUD) | `Controllers/ProductosController.cs` |
| Interfaces y servicios (SOLID) | `Interfaces/IProductoService.cs`, `Services/ProductoService.cs` |
| Registro en el contenedor de DI | `Program.cs` (`AddScoped<IProductoService, ProductoService>`) |
| DTOs y validaciones | `DTOs/` |
| GraphQL (consultas y mutaciones) | `GraphQL/Query.cs`, `GraphQL/Mutation.cs` |
| Base de datos (EF Core Code First, SQLite) | `Data/AppDbContext.cs`, `Migrations/` |
| Swagger | `Program.cs` y la ruta `/swagger` |
| Pruebas de las reglas del sistema | `API_Productos.Tests/ProductoServiceTests.cs` |
| Explicación de SOLID, Clean Code y DI | `README.md` (sección 9) |

## 9. Problemas comunes

| Problema | Solución |
|---|---|
| Aparecen advertencias `NU1903` | Son avisos de seguridad de paquetes. No impiden compilar ni ejecutar |
| `address already in use` al ejecutar | Hay otra instancia corriendo. Cerrarla con `Ctrl + C` |
| Swagger devuelve 400 "JSON inválido" | Al cuerpo le falta cerrar una llave `}` o una coma. Copiar el ejemplo tal cual |
| No veo mis cambios en el código | Detener la API con `Ctrl + C` y volver a ejecutar `dotnet run --launch-profile https` |
| Quiero empezar con la base vacía | Detener la API, borrar el archivo `productos.db` y volver a ejecutar |
