using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;


namespace Papeleria.Infrastructure.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<IEnumerable<CategoryDto>>> ObtenerTodasAsync()
        {
            try
            {
                var categorias = await _context.Categorias
                    .Select(c => new CategoryDto(c.Id, c.Nombre))
                    .ToListAsync();

                return ResultDto<IEnumerable<CategoryDto>>.SuccessResult(categorias);
            }
            catch (Exception ex)
            {
                return ResultDto<IEnumerable<CategoryDto>>.FailureResult("Error al obtener las categorías." + ex);
            }
        }

        public async Task<ResultDto<CategoryDto?>> ObtenerPorIdAsync(Guid id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                    return ResultDto<CategoryDto?>.FailureResult("Categoría no encontrada.", 404);

                var dto = new CategoryDto(categoria.Id, categoria.Nombre);
                return ResultDto<CategoryDto?>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ResultDto<CategoryDto?>.FailureResult("Error al obtener la categoría." + ex);
            }
        }

        public async Task<ResultDto<string>> CrearAsync(CreateCategoriaDto dto)
        {
            try
            {
                var existe = await _context.Categorias.AnyAsync(c => c.Nombre == dto.Nombre);
                if (existe)
                    return ResultDto<string>.FailureResult("Ya existe una categoría con ese nombre.");

                var categoria = new Categoria(dto.Nombre);
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Creación correcta.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al crear la categoría." + ex);
            }
        }

        public async Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateCategoriaDto dto)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                    return ResultDto<string>.FailureResult("Categoría no encontrada.", 404);

                categoria.ActualizarNombre(dto.Nombre);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Categoría actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al actualizar la categoría." + ex);
            }
        }

        public async Task<ResultDto<string>> EliminarAsync(Guid id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                    return ResultDto<string>.FailureResult("Categoría no encontrada.", 404);

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Categoría eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al eliminar la categoría." + ex);
            }
        }
    }
}
