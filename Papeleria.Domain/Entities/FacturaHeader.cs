using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class FacturaHeader : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid UsuarioId { get; private set; }

        public DateTime Fecha { get; private set; } = DateTime.UtcNow;

        public decimal Total { get; private set; }

        public List<FacturaDetalle> Detalles { get; private set; } = new();

        private FacturaHeader() { }

        public FacturaHeader(Guid usuarioId)
        {
            UsuarioId = usuarioId;
            Fecha = DateTime.UtcNow;
        }
        public void AgregarDetalle(Guid productoId, int cantidad, decimal precioUnitario)
        {
            var detalle = new FacturaDetalle(Id, productoId, cantidad, precioUnitario);
            Detalles.Add(detalle);
            Total += detalle.Subtotal;
        }
    }
}
