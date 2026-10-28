using Microsoft.EntityFrameworkCore;
using API_Productos.Data;

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

app.Run();