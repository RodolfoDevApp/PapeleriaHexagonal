using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;


namespace Papeleria.Infrastructure.Services
{
    public class CarroService : ICarroService
    {
        private readonly AppDbContext _context;

        public CarroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<string>> AgregarItemAsync(Guid usuarioId, AgregarItemCarritoDto dto)
        {
            try
            {
                var carrito = await _context.Carros
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null)
                {
                    carrito = new CarroDeCompras(usuarioId);
                    _context.Carros.Add(carrito);
                }

                var producto = await _context.Productos.FindAsync(dto.ProductoId);
                if (producto == null)
                    return ResultDto<string>.FailureResult("Producto no encontrado.");

                carrito.AgregarItem(dto.ProductoId, dto.Cantidad, producto.Precio);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Producto agregado al carrito.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al agregar al carrito."+ ex);
            }
        }

        public async Task<ResultDto<string>> EliminarItemAsync(Guid usuarioId, Guid productoId)
        {
            try
            {
                var carrito = await _context.Carros
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null)
                    return ResultDto<string>.FailureResult("Carrito no encontrado.");

                carrito.EliminarItem(productoId);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Producto eliminado del carrito.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al eliminar del carrito." + ex);
            }
        }

        public async Task<ResultDto<CarritoDto>> ObtenerCarritoAsync(Guid usuarioId)
        {
            try
            {
                var carrito = await _context.Carros
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null)
                    return ResultDto<CarritoDto>.SuccessResult(new CarritoDto { UsuarioId = usuarioId });

                var dto = new CarritoDto
                {
                    UsuarioId = usuarioId,
                    Items = carrito.Items.Select(i => new ItemCarritoDto
                    {
                        ProductoId = i.ProductoId,
                        NombreProducto = i.Producto.Nombre,
                        Cantidad = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario
                    }).ToList()
                };

                return ResultDto<CarritoDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ResultDto<CarritoDto>.FailureResult("Error al obtener el carrito." + ex);
            }
        }

        public async Task<ResultDto<string>> VaciarCarritoAsync(Guid usuarioId)
        {
            try
            {
                var carrito = await _context.Carros
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null)
                    return ResultDto<string>.FailureResult("Carrito no encontrado.");

                carrito.Limpiar();
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Carrito vaciado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al vaciar el carrito." + ex);
            }
        }
        public async Task<ResultDto<FacturaHeader>> ConfirmarCompraAsync(Guid usuarioId)
        {
            try
            {
                var carrito = await _context.Carros
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null || !carrito.Items.Any())
                    return ResultDto<FacturaHeader>.FailureResult("El carrito está vacío.");

                // Crear factura
                var factura = new FacturaHeader(usuarioId);

                foreach (var item in carrito.Items)
                {
                    factura.AgregarDetalle(item.ProductoId, item.Cantidad, item.PrecioUnitario);
                }

                // Guardar factura
                _context.Facturas.Add(factura);

                // Eliminar carrito
                _context.Carros.Remove(carrito);

                await _context.SaveChangesAsync();

                return ResultDto<FacturaHeader>.SuccessResult(factura,"Compra confirmada. Factura generada correctamente.");
            }
            catch (Exception ex) 
            {
                return ResultDto<FacturaHeader>.FailureResult("Error al confirmar la compra." + ex);
            }
        }

    }

}
