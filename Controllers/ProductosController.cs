using API_Productos.DTOs;
using API_Productos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            var productos = await _productoService.GetProductosAsync();
            return Ok(productos);
        }

        // GET: api/productos/
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> PostProducto([FromBody] CrearProductoDto crearProductoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoProducto = await _productoService.CreateProductoAsync(crearProductoDto);
            return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        // PUT: api/productos/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] CrearProductoDto crearProductoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actualizado = await _productoService.UpdateProductoAsync(id, crearProductoDto);

            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/productos/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var eliminado = await _productoService.DeleteProductoAsync(id);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}