namespace Papeleria.WebApi.DTOs
{
    public class ProductoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int TiempoElaboracionDias { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;

        public ProductoDto(
            Guid id,
            string nombre,
            string descripcion,
            decimal precio,
            int tiempoElaboracionDias,
            string imagenUrl,
            string categoriaNombre)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            TiempoElaboracionDias = tiempoElaboracionDias;
            ImagenUrl = imagenUrl;
            CategoriaNombre = categoriaNombre;
        }
    }

}

