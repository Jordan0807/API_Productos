using API_Productos.Data;
using API_Productos.Interfaces;
using API_Productos.Models;

namespace API_Productos.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        // Inyectamos el AppDbContext mediante el constructor
        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Producto> GetProductos()
        {
            return _context.Productos;
        }
    }
}