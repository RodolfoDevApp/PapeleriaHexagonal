using System.ComponentModel.DataAnnotations;

namespace Papeleria.WebApi.DTOs
{
    public class CreateProductoDto : IValidatableObject
    {
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Nombre' no debe superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo 'Descripción' es obligatorio.")]
        [StringLength(300, ErrorMessage = "La 'Descripción' no debe superar los 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 999999, ErrorMessage = "El 'Precio' debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Range(1, 365, ErrorMessage = "El 'Tiempo de elaboración' debe ser entre 1 y 365 días.")]
        public int TiempoElaboracionDias { get; set; }

        [Required(ErrorMessage = "La 'URL de imagen' es obligatoria.")]
        [Url(ErrorMessage = "La 'URL de imagen' no tiene un formato válido.")]
        public string ImagenUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificarse una categoría válida.")]
        public Guid CategoriaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Precio % 0.01m != 0)
                yield return new ValidationResult("El 'Precio' debe tener como máximo dos decimales.", new[] { nameof(Precio) });

            if (TiempoElaboracionDias > 60)
                yield return new ValidationResult("El 'Tiempo de elaboración' no debe superar los 60 días.", new[] { nameof(TiempoElaboracionDias) });
        }
    }

}
