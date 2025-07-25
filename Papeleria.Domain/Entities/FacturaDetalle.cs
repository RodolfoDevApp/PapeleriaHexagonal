using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class FacturaDetalle : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid FacturaId { get; private set; }

        public Guid ProductoId { get; private set; }
        public Producto Producto { get; private set; } = default!;

        public int Cantidad { get; private set; }

        public decimal PrecioUnitario { get; private set; }

        public decimal Subtotal => PrecioUnitario * Cantidad;


        private FacturaDetalle() { }

        public FacturaDetalle(Guid facturaId, Guid productoId, int cantidad, decimal precioUnitario)
        {
            FacturaId = facturaId;
            ProductoId = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
    }
}
