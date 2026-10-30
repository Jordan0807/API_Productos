using System.ComponentModel.DataAnnotations;

namespace API_Productos.DTOs
{
    public class CrearProductoDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; } // Opcional o requerida según prefieras

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }
    }
}