using API_Productos.DTOs;

namespace API_Productos.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetProductosAsync();
        Task<ProductoDto?> GetProductoByIdAsync(int id);
        Task<ProductoDto> CreateProductoAsync(CrearProductoDto crearProductoDto);
        Task<bool> UpdateProductoAsync(int id, CrearProductoDto crearProductoDto);
        Task<bool> DeleteProductoAsync(int id);
    }
}