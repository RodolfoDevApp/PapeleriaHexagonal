using Humanizer;
using Microsoft.EntityFrameworkCore;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;


namespace Papeleria.Infrastructure.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductoDto>> ObtenerTodosAsync(string? busqueda, Guid? categoriaId, int page, int pageSize)
        {
            try
            {

            var query = _context.Productos.Include(p => p.Categoria).AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
                query = query.Where(p => p.Nombre.Contains(busqueda) || p.Descripcion.Contains(busqueda));

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId);

            var totalItems = await query.CountAsync();
            var productos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = productos.Select(p => new ProductoDto(
                p.Id,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.TiempoElaboracionDias,
                p.ImagenUrl,
                p.Categoria.Nombre
            )).ToList();

            return PagedResult<ProductoDto>.SuccessPagedResult(data, totalItems, page, pageSize, "Productos obtenidos correctamente.");
            }catch (Exception ex)
            {
                return PagedResult<ProductoDto>.FailurePagedResult("Error al crear el producto." + ex);
            }
        }

        public async Task<ResultDto<ProductoDto?>> ObtenerPorIdAsync(Guid id)
        {
            try
            {

            var producto = await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return ResultDto<ProductoDto?>.FailureResult("Producto no encontrado.", 404);

            var dto = new ProductoDto(
                producto.Id,
                producto.Nombre,
                producto.Descripcion,
                producto.Precio,
                producto.TiempoElaboracionDias,
                producto.ImagenUrl,
                producto.Categoria.Nombre
            );

            return ResultDto<ProductoDto?>.SuccessResult(dto, "Producto obtenido correctamente.");
            }
            catch (Exception ex)
            {

                return ResultDto<ProductoDto?>.FailureResult("Error al obtener producto." + ex);
            }
        }

        public async Task<ResultDto<string>> CrearAsync(CreateProductoDto dto)
        {
            try
            {
                var producto = new Producto(
                    dto.Nombre,
                    dto.Descripcion,
                    dto.Precio,
                    dto.CategoriaId,
                    dto.TiempoElaboracionDias,
                    dto.ImagenUrl
                );

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Creación correcta.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al crear el producto." +ex);
            }
        }

        public async Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateProductoDto dto)
        {
            try
            {

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return ResultDto<string>.FailureResult("Producto no encontrado.", 404);

            producto.Actualizar(dto.Nombre, dto.Descripcion, dto.Precio, dto.CategoriaId, dto.TiempoElaboracionDias, dto.ImagenUrl);
            await _context.SaveChangesAsync();

            return ResultDto<string>.SuccessResult("Actualización correcta.");
            }
            catch (Exception ex)
            {

                return ResultDto<string>.FailureResult("Error al Actualizar." + ex);
            }
        }

        public async Task<ResultDto<string>> EliminarAsync(Guid id)
        {
            try
            {

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return ResultDto<string>.FailureResult("Producto no encontrado.", 404);

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return ResultDto<string>.SuccessResult("Eliminación correcta.");
            }
            catch (Exception ex)
            {

                return ResultDto<string>.FailureResult("Error al eliminar." + ex);
            }
        }
    }
}
