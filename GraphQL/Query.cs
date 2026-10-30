using API_Productos.DTOs;
using API_Productos.Interfaces;
using HotChocolate;

namespace API_Productos.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<ProductoDto>> GetProductos([Service] IProductoService productoService) =>
            productoService.GetProductosAsync();

        public Task<ProductoDto?> GetProductoPorId(int id, [Service] IProductoService productoService) =>
            productoService.GetProductoByIdAsync(id);
    }
}