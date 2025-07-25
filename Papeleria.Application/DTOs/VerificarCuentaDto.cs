using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.DTOs
{
    public class VerificarCuentaDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
