using Papeleria.Application.DTOs;
using Papeleria.WebApi.DTOs;


namespace Papeleria.Application.Interfaces
{
    public interface IProductoService
    {
        Task<PagedResult<ProductoDto>> ObtenerTodosAsync(string? busqueda = null, Guid? categoriaId = null, int page = 1, int pageSize = 10);
        Task<ResultDto<ProductoDto?>> ObtenerPorIdAsync(Guid id);
        Task<ResultDto<string>> CrearAsync(CreateProductoDto dto);
        Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateProductoDto dto);
        Task<ResultDto<string>> EliminarAsync(Guid id);
    }
}
