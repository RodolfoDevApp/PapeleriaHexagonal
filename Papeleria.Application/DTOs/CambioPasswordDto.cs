using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.DTOs
{
    public class CambioPasswordDto
    {
        public Guid UsuarioId { get; set; }
        public string NuevoPassword { get; set; } = string.Empty;
    }
}
