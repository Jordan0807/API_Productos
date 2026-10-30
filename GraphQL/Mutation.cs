using API_Productos.DTOs;
using API_Productos.Interfaces;
using HotChocolate;

namespace API_Productos.GraphQL
{
    public class Mutation
    {
        public async Task<ProductoDto> CrearProducto(
            CrearProductoDto input,
            [Service] IProductoService productoService)
        {
            return await productoService.CreateProductoAsync(input);
        }

        public async Task<bool> ActualizarProducto(
            int id,
            CrearProductoDto input,
            [Service] IProductoService productoService)
        {
            return await productoService.UpdateProductoAsync(id, input);
        }

        public async Task<bool> EliminarProducto(
            int id,
            [Service] IProductoService productoService)
        {
            return await productoService.DeleteProductoAsync(id);
        }
    }
}