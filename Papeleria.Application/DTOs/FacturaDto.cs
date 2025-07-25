

namespace Papeleria.Application.DTOs
{
    public class FacturaDto
    {
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<FacturaDetalleDto> Detalles { get; set; } = new();
    }
}
