﻿
using System.ComponentModel.DataAnnotations;


namespace Papeleria.Application.DTOs
{
    public class UpdateCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}
