using Papeleria.Application.DTOs;
using Papeleria.Domain.Entities;
using Papeleria.WebApi.DTOs;


namespace Papeleria.Application.Interfaces
{
    public interface ICarroService
    {
        Task<ResultDto<string>> AgregarItemAsync(Guid usuarioId, AgregarItemCarritoDto dto);
        Task<ResultDto<string>> EliminarItemAsync(Guid usuarioId, Guid productoId);
        Task<ResultDto<CarritoDto>> ObtenerCarritoAsync(Guid usuarioId);
        Task<ResultDto<string>> VaciarCarritoAsync(Guid usuarioId);
        Task<ResultDto<FacturaHeader>> ConfirmarCompraAsync(Guid usuarioId);

    }
}
