using System.ComponentModel.DataAnnotations;

namespace Papeleria.WebApi.DTOs
{
    public class CreateCategoriaDto
    {
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Nombre' no debe superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
