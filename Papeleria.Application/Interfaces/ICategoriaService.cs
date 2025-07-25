using Papeleria.Application.DTOs;
using Papeleria.WebApi.DTOs;


namespace Papeleria.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<ResultDto<IEnumerable<CategoryDto>>> ObtenerTodasAsync();
        Task<ResultDto<CategoryDto?>> ObtenerPorIdAsync(Guid id);
        Task<ResultDto<string>> CrearAsync(CreateCategoriaDto dto);
        Task<ResultDto<string>> ActualizarAsync(Guid id, UpdateCategoriaDto dto);
        Task<ResultDto<string>> EliminarAsync(Guid id);
    }
}
