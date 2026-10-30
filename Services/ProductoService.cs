using System.ComponentModel.DataAnnotations;
using API_Productos.Data;
using API_Productos.DTOs;
using API_Productos.Interfaces;
using API_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Productos.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDto>> GetProductosAsync()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return productos.Select(ToDto);
        }

        public async Task<ProductoDto?> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            return producto is null ? null : ToDto(producto);
        }

        public async Task<ProductoDto> CreateProductoAsync(CrearProductoDto dto)
        {
            Validar(dto);

            var producto = new Producto();
            Aplicar(producto, dto);

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return ToDto(producto);
        }

        public async Task<bool> UpdateProductoAsync(int id, CrearProductoDto dto)
        {
            Validar(dto);

            var producto = await _context.Productos.FindAsync(id);
            if (producto is null) return false;

            Aplicar(producto, dto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto is null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        // Las reglas viven acá para que valgan tanto en REST como en GraphQL.
        private static void Validar(CrearProductoDto dto) =>
            Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);

        private static void Aplicar(Producto producto, CrearProductoDto dto)
        {
            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion ?? string.Empty;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
        }

        private static ProductoDto ToDto(Producto p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock
        };
    }
}