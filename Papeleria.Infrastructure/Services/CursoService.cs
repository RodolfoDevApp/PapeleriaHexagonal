using Microsoft.EntityFrameworkCore;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;


namespace Papeleria.Infrastructure.Services
{
    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;

        public CursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CursoDto>> ObtenerTodosAsync(string? busqueda = null, Guid? categoriaId = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Cursos
                    .Include(c => c.Categoria)
                    .Where(c => c.Activo);

                if (!string.IsNullOrWhiteSpace(busqueda))
                    query = query.Where(c => c.Titulo.Contains(busqueda));

                if (categoriaId.HasValue)
                    query = query.Where(c => c.CategoriaId == categoriaId.Value);

                var totalItems = await query.CountAsync();

                var cursos = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CursoDto(
                        c.Id, c.Titulo, c.Descripcion, c.Precio, c.Lugar, c.Fecha,
                        c.DuracionHoras, c.ImagenUrl, c.Categoria.Nombre
                    ))
                    .ToListAsync();

                return PagedResult<CursoDto>.SuccessPagedResult(cursos, totalItems, page, pageSize);
            }
            catch (Exception ex)
            {
                return PagedResult<CursoDto>.FailurePagedResult("Error al obtener los cursos." + ex);
            }
        }

        public async Task<ResultDto<CursoDto?>> ObtenerPorIdAsync(Guid id)
        {
            try
            {
                var curso = await _context.Cursos
                    .Include(c => c.Categoria)
                    .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

                if (curso == null)
                    return ResultDto<CursoDto?>.FailureResult("Curso no encontrado.", 404);

                var dto = new CursoDto(
                    curso.Id, curso.Titulo, curso.Descripcion, curso.Precio,
                    curso.Lugar, curso.Fecha, curso.DuracionHoras, curso.ImagenUrl,
                    curso.Categoria.Nombre
                );

                return ResultDto<CursoDto?>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ResultDto<CursoDto?>.FailureResult("Error al obtener el curso." + ex);
            }
        }

        public async Task<ResultDto<string>> CrearAsync(CreateCursoDto dto)
        {
            try
            {
                var curso = new Curso(
                    dto.Titulo, dto.Descripcion, dto.Precio, dto.CategoriaId,
                    dto.Lugar, dto.Fecha, dto.DuracionHoras, dto.ImagenUrl
                );

                _context.Cursos.Add(curso);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Creación correcta.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al crear el curso." + ex);
            }
        }

        public async Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateCursoDto dto)
        {
            try
            {
                var curso = await _context.Cursos.FindAsync(id);
                if (curso == null)
                    return ResultDto<string>.FailureResult("Curso no encontrado.", 404);

                curso.Actualizar(dto.Titulo, dto.Descripcion, dto.Precio, dto.CategoriaId,
                                 dto.Lugar, dto.Fecha, dto.DuracionHoras, dto.ImagenUrl);

                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Curso actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al actualizar el curso." + ex);
            }
        }

        public async Task<ResultDto<string>> EliminarAsync(Guid id)
        {
            try
            {
                var curso = await _context.Cursos.FindAsync(id);
                if (curso == null)
                    return ResultDto<string>.FailureResult("Curso no encontrado.", 404);

                curso.Cancelar();
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Curso eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al eliminar el curso." + ex);
            }
        }
    }
}
