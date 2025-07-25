using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.DTOs
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public UsuarioDto() { }

        public UsuarioDto(Guid id, string nombre, string email, string rol, bool activo, DateTime fechaRegistro)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
            Rol = rol;
            Activo = activo;
            FechaRegistro = fechaRegistro;
        }
    }
}
