using API_Productos.Models;

namespace API_Productos.Interfaces
{
    public interface IProductoService
    {
        IQueryable<Producto> GetProductos();
    }
}