using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class CarroDeCompras : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UsuarioId { get; private set; }
        public List<CarroItem> Items { get; private set; } = new();

        public CarroDeCompras(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public void AgregarItem(Guid productoId, int cantidad, decimal precioUnitario)
        {
            var existente = Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (existente != null)
            {
                existente.IncrementarCantidad(cantidad);
            }
            else
            {
                Items.Add(new CarroItem(productoId, cantidad, precioUnitario));
            }
        }

        public void EliminarItem(Guid productoId)
        {
            var item = Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void Limpiar()
        {
            Items.Clear();
        }

        public decimal CalcularTotal() =>
            Items.Sum(i => i.Cantidad * i.PrecioUnitario);
    }

}
