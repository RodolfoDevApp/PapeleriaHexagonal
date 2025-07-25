using Papeleria.Application.DTOs;
using Papeleria.WebApi.DTOs;

namespace Papeleria.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResultDto<string>> RegistrarAsync(UsuarioRegistroDto dto);
        Task<ResultDto<string>> LoginAsync(UsuarioLoginDto dto);
        Task<ResultDto<string>> CambiarPasswordAsync(Guid usuarioId, CambioPasswordDto dto);
        Task<ResultDto<string>> EnviarRecuperacionPasswordAsync(string email);
        Task<ResultDto<string>> RecuperarPasswordAsync(RecuperarPasswordDto dto);
        Task<ResultDto<string>> ActivarUsuarioAsync(Guid id);
        Task<ResultDto<string>> DesactivarUsuarioAsync(Guid id);
        Task<ResultDto<string>> CambiarRolAsync(Guid usuarioId, string nuevoRol);
        Task<ResultDto<string>> ActualizarAsync(Guid id, ActualizarUsuarioDto dto);
        Task<ResultDto<IEnumerable<UsuarioDto>>> ObtenerTodosAsync();
        Task<ResultDto<UsuarioDto?>> ObtenerPorIdAsync(Guid id);
        Task<ResultDto<string>> EnviarVerificacionCuentaAsync(string email);
        Task<ResultDto<string>> VerificarCuentaAsync(string token);
    }
}
