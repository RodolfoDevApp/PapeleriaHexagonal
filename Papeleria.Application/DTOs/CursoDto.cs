namespace Papeleria.WebApi.DTOs
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int DuracionHoras { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        public CursoDto(Guid id, string titulo, string descripcion, decimal precio, string lugar, DateTime fecha, int duracionHoras, string imagenUrl, string categoria)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            Precio = precio;
            Lugar = lugar;
            Fecha = fecha;
            DuracionHoras = duracionHoras;
            ImagenUrl = imagenUrl;
            Categoria = categoria;
        }
    }

}
