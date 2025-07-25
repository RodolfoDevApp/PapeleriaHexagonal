using Papeleria.Application.DTOs;
using Papeleria.WebApi.DTOs;

namespace Papeleria.Application.Interfaces
{
    public interface ICursoService
    {
        Task<PagedResult<CursoDto>> ObtenerTodosAsync(string? busqueda = null, Guid? categoriaId = null, int page = 1, int pageSize = 10);
        Task<ResultDto<CursoDto?>> ObtenerPorIdAsync(Guid id);
        Task<ResultDto<string>> CrearAsync(CreateCursoDto dto);
        Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateCursoDto dto);
        Task<ResultDto<string>> EliminarAsync(Guid id);
    }
}
