namespace API_Productos.Models
{
    public class Producto
    {
        private decimal _precio;
        private int _stock;

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio
        {
            get { return _precio; }
            set
            {
                if (value < 0)
                {
                    _precio = 0;
                }
                else
                {
                    _precio = value;
                }
            }
        }

        public int Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0)
                {
                    _stock = 0;
                }
                else
                {
                    _stock = value;
                }
            }
        }
    }
}