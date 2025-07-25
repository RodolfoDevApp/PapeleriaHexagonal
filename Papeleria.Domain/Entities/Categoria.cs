using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class Categoria : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Nombre { get; private set; } = string.Empty;

        // Constructor EF
        private Categoria() { }

        public Categoria(string nombre)
        {
            Nombre = nombre;
        }
        public void ActualizarNombre(string nuevoNombre)
        {
            Nombre = nuevoNombre;
        }
    }
}
