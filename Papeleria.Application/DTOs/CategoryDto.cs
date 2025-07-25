
namespace Papeleria.WebApi.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public CategoryDto() { }

        public CategoryDto(Guid id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}


