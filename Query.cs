using API_Productos.Interfaces;
using API_Productos.Models;


public class Query
{
    // Ahora inyectamos la interfaz en lugar del DbContext directo
    public IQueryable<Producto> GetProductos([Service] IProductoService productoService) =>
        productoService.GetProductos();
}