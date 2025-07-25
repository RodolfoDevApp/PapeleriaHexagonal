using System.ComponentModel.DataAnnotations;

namespace Papeleria.WebApi.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCursoDto : IValidatableObject
    {
        [Required(ErrorMessage = "El campo 'Título' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Título' no debe superar los 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo 'Descripción' es obligatorio.")]
        [StringLength(300, ErrorMessage = "La 'Descripción' no debe superar los 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 999999, ErrorMessage = "El 'Precio' debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo 'Lugar' es obligatorio.")]
        [StringLength(150, ErrorMessage = "El 'Lugar' no debe superar los 150 caracteres.")]
        public string Lugar { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificarse una 'Fecha' válida.")]
        public DateTime Fecha { get; set; }

        [Range(1, 480, ErrorMessage = "La 'Duración' debe estar entre 1 y 480 horas.")]
        public int DuracionHoras { get; set; }

        [Required(ErrorMessage = "La 'URL de imagen' es obligatoria.")]
        [Url(ErrorMessage = "La 'URL de imagen' no tiene un formato válido.")]
        public string ImagenUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificarse una categoría válida.")]
        public Guid CategoriaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Precio % 0.01m != 0)
                yield return new ValidationResult("El 'Precio' debe tener como máximo dos decimales.", new[] { nameof(Precio) });

            if (Fecha.Date < DateTime.Today)
                yield return new ValidationResult("La 'Fecha' del curso no puede estar en el pasado.", new[] { nameof(Fecha) });
        }
    }

}
