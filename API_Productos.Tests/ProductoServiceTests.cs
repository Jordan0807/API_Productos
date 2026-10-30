using System.ComponentModel.DataAnnotations;
using API_Productos.Data;
using API_Productos.DTOs;
using API_Productos.Services;
using Microsoft.EntityFrameworkCore;

namespace API_Productos.Tests
{
    public class ProductoServiceTests
    {
        private static ProductoService CrearServicio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ProductoService(new AppDbContext(options));
        }

        private static CrearProductoDto ProductoValido() => new()
        {
            Nombre = "Teclado",
            Descripcion = "Mecánico",
            Precio = 100m,
            Stock = 5
        };

        [Fact]
        public async Task Crear_ProductoValido_SePuedeObtenerPorId()
        {
            var servicio = CrearServicio();

            var creado = await servicio.CreateProductoAsync(ProductoValido());
            var obtenido = await servicio.GetProductoByIdAsync(creado.Id);

            Assert.NotNull(obtenido);
            Assert.Equal("Teclado", obtenido.Nombre);
        }

        [Fact]
        public async Task Crear_PrecioNegativo_LanzaValidationException()
        {
            var servicio = CrearServicio();
            var dto = ProductoValido();
            dto.Precio = -10m;

            await Assert.ThrowsAsync<ValidationException>(() => servicio.CreateProductoAsync(dto));
        }

        [Fact]
        public async Task Crear_StockNegativo_LanzaValidationException()
        {
            var servicio = CrearServicio();
            var dto = ProductoValido();
            dto.Stock = -1;

            await Assert.ThrowsAsync<ValidationException>(() => servicio.CreateProductoAsync(dto));
        }

        [Fact]
        public async Task Crear_NombreVacio_LanzaValidationException()
        {
            var servicio = CrearServicio();
            var dto = ProductoValido();
            dto.Nombre = "";

            await Assert.ThrowsAsync<ValidationException>(() => servicio.CreateProductoAsync(dto));
        }

        [Fact]
        public async Task Obtener_ProductoInexistente_DevuelveNull()
        {
            var servicio = CrearServicio();

            Assert.Null(await servicio.GetProductoByIdAsync(999));
        }

        [Fact]
        public async Task Actualizar_ProductoInexistente_DevuelveFalse()
        {
            var servicio = CrearServicio();

            Assert.False(await servicio.UpdateProductoAsync(999, ProductoValido()));
        }

        [Fact]
        public async Task Eliminar_ProductoInexistente_DevuelveFalse()
        {
            var servicio = CrearServicio();

            Assert.False(await servicio.DeleteProductoAsync(999));
        }

        [Fact]
        public async Task Eliminar_ProductoExistente_LoQuitaDelListado()
        {
            var servicio = CrearServicio();
            var creado = await servicio.CreateProductoAsync(ProductoValido());

            var eliminado = await servicio.DeleteProductoAsync(creado.Id);
            var productos = await servicio.GetProductosAsync();

            Assert.True(eliminado);
            Assert.Empty(productos);
        }
    }
}