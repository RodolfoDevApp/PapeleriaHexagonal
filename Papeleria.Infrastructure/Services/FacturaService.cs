using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Papeleria.Infrastructure.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly AppDbContext _context;
        private readonly ICarroService _carroRepo;

        public FacturaService(AppDbContext context, ICarroService carroRepo)
        {
            _context = context;
            _carroRepo = carroRepo;
        }

        public async Task<ResultDto<FacturaDto>> ConfirmarCompraAsync(Guid usuarioId)
        {
            try
            {
                var carro = await _carroRepo.ObtenerCarritoAsync(usuarioId);
                if (carro is null || !carro.Data.Items.Any())
                    return ResultDto<FacturaDto>.FailureResult("Carrito vacío.");

               
                var factura = new FacturaHeader(usuarioId);
                foreach (var item in carro.Data.Items)
                {
                    var detalle = new FacturaDetalle(factura.Id, item.ProductoId, item.Cantidad, item.PrecioUnitario);
                    factura.Detalles.Add(detalle);
                }

                _context.Facturas.Add(factura);
                await _carroRepo.VaciarCarritoAsync(usuarioId); // Limpia el carrito
                await _context.SaveChangesAsync();

                var dto = new FacturaDto
                {
                    Id = factura.Id,
                    Fecha = factura.Fecha,
                    Total = factura.Total,
                    Detalles = factura.Detalles.Select(d => new FacturaDetalleDto
                    {
                        ProductoId = d.ProductoId,
                        NombreProducto = d.Producto.Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                };

                return ResultDto<FacturaDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ResultDto<FacturaDto>.FailureResult("Error al generar factura: " + ex.Message);
            }
        }
    }

}
