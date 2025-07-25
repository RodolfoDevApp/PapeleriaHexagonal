using System;
using Papeleria.Domain.Entities;

namespace Papeleria.Domain.Entities
{
    public class CarroItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductoId { get; private set; }
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }

        // ✅ Propiedad de navegación
        public Producto Producto { get; private set; } = default!;

        private CarroItem() { }

        public CarroItem(Guid productoId, int cantidad, decimal precioUnitario)
        {
            ProductoId = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public void IncrementarCantidad(int cantidad)
        {
            Cantidad += cantidad;
        }

        public void CambiarCantidad(int cantidad)
        {
            Cantidad = cantidad;
        }
    }
}
