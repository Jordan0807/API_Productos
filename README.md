# API_Productos
Proyecto Roque Taller
# 🛒 API_Productos

Proyecto integrador desarrollado en **.NET 8** aplicando Programación Orientada a Objetos (POO), principios **SOLID**, Inyección de Dependencias, Entity Framework Core (Code First), API REST y GraphQL.

---

## 📂 1. Arquitectura y Organización del Proyecto

El proyecto está organizado en capas para separar las responsabilidades de manera limpia y mantenible:

- **`Controllers/`**: Expone los endpoints de la API REST (CRUD completo de productos).
- **`Data/`**: Contiene el `DbContext` y maneja la persistencia con Entity Framework Core (Code First).
- **`DTOs/`**: Objetos de transferencia de datos para desacoplar las entidades de la base de datos de lo que se expone al cliente.
- **`GraphQL/`**: Implementa el esquema, consultas (*Queries*) y mutaciones (*Mutations*).
- **`Interfaces/`**: Define los contratos (*abstracciones*) para aplicar inversión de dependencias.
- **`Models/`**: Contiene la entidad principal `Producto` con encapsulamiento y validaciones de negocio.
- **`Services/`**: Contiene la lógica de negocio pura de la aplicación.

---

## 🛠️ 2. Requisitos Técnicos Implementados

- **ASP.NET Core Web API**: Endpoints REST para listar, buscar por ID, crear, actualizar y eliminar productos.
- **Entity Framework Core (Code First)**: Mapeo automático de clases a la base de datos (SQLite/SQL Server).
- **Inyección de Dependencias (DI)**: Los servicios se inyectan a través de interfaces configuradas en el contenedor de .NET.
- **Principios SOLID**: Aplicados mediante el uso de interfaces, clases con responsabilidad única y desacoplamiento.
- **Swagger**: Documentación interactiva para probar los endpoints de forma visual.

---

## 🗣️ 3. Guía de Defensa para el Examen (Preguntas Frecuentes)

Si el profesor te pide justificar las decisiones de diseño durante la presentación, puedes explicar lo siguiente:

### ¿Por qué el proyecto está dividido en estas carpetas?
> *"Separamos las responsabilidades para cumplir con el principio de **Responsabilidad Única (SRP)** de SOLID y mantener una arquitectura limpia. Cada carpeta tiene un propósito específico: los modelos guardan los datos, los servicios procesan la lógica, los repositorios hablan con la base de datos y los controladores manejan las peticiones HTTP."*

### ¿Por qué utilizaste Interfaces y Inyección de Dependencias?
> *"Para cumplir con el **Principio de Inversión de Dependencias (DIP)**. Los controladores no dependen de clases concretas, sino de abstracciones (interfaces como `IProductoService`). Esto hace que el código sea más flexible, fácil de probar y mantenible si necesitamos cambiar la implementación en el futuro sin romper todo el sistema."*

### ¿Para qué sirven los DTOs?
> *"Los DTOs (*Data Transfer Objects*) nos permiten controlar exactamente qué datos enviamos y recibimos desde la API, protegiendo las entidades internas del dominio y evitando exponer campos sensibles o innecesarios hacia el exterior."*

### ¿Cuál es el flujo de la aplicación?
> *"El flujo comienza cuando el cliente hace una petición (por ejemplo, vía Postman o Swagger a través de un controlador REST, o mediante GraphQL). El controlador delega la petición al servicio, el servicio aplica las reglas de negocio utilizando las interfaces, y finalmente la capa de datos utiliza Entity Framework Core para guardar o consultar la información en la base de datos."*