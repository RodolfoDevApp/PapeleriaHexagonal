

using Papeleria.Application.Interfaces;

namespace Papeleria.Application.DTOs
{
    public class CarritoDto
    {
        public Guid UsuarioId { get; set; }
        public List<ItemCarritoDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(x => x.Subtotal);
    }

}
