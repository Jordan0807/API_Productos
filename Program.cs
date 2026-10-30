using API_Productos.Data;
using API_Productos.Interfaces;
using API_Productos.Services;
using API_Productos.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios de controladores
builder.Services.AddControllers();

// 2. Configurar Swagger para documentar y probar la API (Requerimiento obligatorio)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Registrar tu AppDbContext con SQLite usando la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CadenaSQL"))
);

builder.Services.AddScoped<IProductoService, ProductoService>();

// Configuración de GraphQL

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// 4. Configurar el pipeline HTTP y habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();